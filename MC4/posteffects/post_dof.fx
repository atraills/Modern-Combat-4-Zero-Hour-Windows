float4 ClearColor : DIFFUSE = { 1.0f, 0.0f, 0.0f, 1.0f};
float4 ClearColor1= { 0.0f, 0.0f, 0.0f, 0.0f};
float ClearDepth = 1.0f;

float Script : STANDARDSGLOBAL
<
#ifdef HKG_DX11
	string Script = "Technique=DOF11;";
#elif defined(HKG_DX10)
	string Script = "Technique=DOF10;";
#else
	string Script = "Technique=DOF;";
#endif
>;

float4 WindowSize : VIEWPORTPIXELSIZE;

float BlurWidth <
    string UIName = "Blur width";
    string UIWidget = "slider";
    float UIMin = 0.0f;
    float UIMax = 10.0f;
    float UIStep = 0.5f;
> = 2.0f;


uniform float  g_fFocalPlaneDistance   = 3;// Focal plane distance
uniform float  g_fNearBlurPlaneDistance = 1;// Near blur plance distance
uniform float  g_fFarBlurPlaneDistance  = 10;// Far blur plane distance
uniform float  g_fFarBlurLimit          = 1;//Far blur limit [0,1]
uniform float  g_fMaxDiscRadius 		= 0.5f;

static const float NUM_POISSON_TAPS = 8;
static const float2 g_Poisson[8] =  
{
    float2( 0.000000f, 0.000000f ),
    float2( 0.527837f,-0.085868f ),
    float2(-0.040088f, 0.536087f ),
    float2(-0.670445f,-0.179949f ),
    float2(-0.419418f,-0.616039f ),
    float2( 0.440453f,-0.639399f ),
    float2(-0.757088f, 0.349334f ),
    float2( 0.574619f, 0.685879f ),
};


// blur filter weights
const half weights7[7] = {
	0.05,
	0.1,
	0.2,
	0.3,
	0.2,
	0.1,
	0.05,
};

float2 g_vMaxCoC = float2( 0.03f, 0.01f );
static const float  g_fRadiusScale = 1.0f;


texture SceneMap : RENDERCOLORTARGET
<
	string source = "SCENE";
>;

sampler SceneSampler = sampler_state 
{
    texture = <SceneMap>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
	FILTER = MIN_MAG_MIP_POINT;
};

texture ViewDepthMap : RENDERCOLORTARGET
< 
	string source = "DEPTH";
>;

sampler ViewDepthMapSampler = sampler_state 
{
    texture = <ViewDepthMap>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
 	FILTER = MIN_MAG_LINEAR_MIP_POINT;
//   MIPFILTER = NONE;
//    MINFILTER = LINEAR;
//    MAGFILTER = LINEAR;
};


texture HBlurMap : RENDERCOLORTARGET
< 
    float2 ViewportRatio = { 0.25, 0.25 };
    int MIPLEVELS = 1;
    string format = "A8R8G8B8";
>;

sampler HBlurSampler = sampler_state 
{
    texture = <HBlurMap>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
	FILTER = MIN_MAG_LINEAR_MIP_POINT;
};

texture FinalBlurMap : RENDERCOLORTARGET
< 
    float2 ViewportRatio = { 0.25, 0.25 };
    int MIPLEVELS = 1;
    string format = "A8R8G8B8";
>;

sampler FinalBlurSampler = sampler_state 
{
    texture = <FinalBlurMap>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
	FILTER = MIN_MAG_LINEAR_MIP_POINT;
};

///////////////////////////////////////////////////////////
/////////////////////////////////// data structures ///////
///////////////////////////////////////////////////////////

struct VS_OUTPUT_BLUR
{
    float4 Position   : POSITION;
    float2 TexCoord0 : TEXCOORD0;
    float4 TexCoord12: TEXCOORD1;
    float4 TexCoord34: TEXCOORD2;
    float4 TexCoord56: TEXCOORD3;
};

struct VS_OUTPUT
{
   	float4 Position   : POSITION;
    float2 TexCoord0  : TEXCOORD0;
    float2 TexCoord1  : TEXCOORD1;
};

////////////////////////////////////////////////////////////
////////////////////////////////// vertex shaders //////////
////////////////////////////////////////////////////////////



// generate texcoords for blur
VS_OUTPUT_BLUR VS_Blur(float4 Position : POSITION, 
					   float2 TexCoord : TEXCOORD0,
					   uniform int nsamples,
					   uniform float2 direction
					   )
{
    VS_OUTPUT_BLUR OUT = (VS_OUTPUT_BLUR)0;
	float2 texelSize = 1 / WindowSize;

	OUT.Position = Position;
	
#ifdef HKG_DX10
	// DX9 has hald texel offset already (so we get some linear bluring)
	// DX10 is centered, so we can offset the lookup texels 
	TexCoord.xy += direction*float2( -texelSize.x, -texelSize.y)*0.5 ;
#endif

	float2 blurDir = BlurWidth*texelSize*direction;
    float2 s = TexCoord - (nsamples-1)*0.5*blurDir;
   
	OUT.TexCoord0 = s;
	OUT.TexCoord12.xy = s + (blurDir*1);
    OUT.TexCoord12.zw = s + (blurDir*2);
    OUT.TexCoord34.xy = s + (blurDir*3);
    OUT.TexCoord34.zw = s + (blurDir*4);
    OUT.TexCoord56.xy = s + (blurDir*5);
    OUT.TexCoord56.zw = s + (blurDir*6);
    return OUT;
}

VS_OUTPUT VS_Quad(float4 Position : POSITION, 
				  float2 TexCoord : TEXCOORD0)
{
    VS_OUTPUT OUT;
	OUT.Position = Position; 

		float2 texelSize = 1.0 / WindowSize;
#ifndef HKG_DX10
	float2 dxHalfTexelPos = float2( -texelSize.x, texelSize.y ); // [-1,1] -> WindowSize  ==  2/WindowSize,  so half texel = 1/windowize
	OUT.Position.xy += (dxHalfTexelPos * Position.w);
#endif
	
	OUT.TexCoord0 = TexCoord;
    OUT.TexCoord1 = TexCoord;
 	
	return OUT;
}

//////////////////////////////////////////////////////
////////////////////////////////// pixel shaders /////
//////////////////////////////////////////////////////

float ComputeDepthBlur( FLOAT fDepth )
{
    // Compute depth blur
    float fDepthBlur;
    
    if( fDepth < g_fFocalPlaneDistance )
    {
        // Scale depth value between near blur distance and focal distance to [-1,0] range
        fDepthBlur = ( fDepth - g_fFocalPlaneDistance ) / ( g_fFocalPlaneDistance - g_fNearBlurPlaneDistance );
    }
    else
    {
        // Scale depth value between focal distance and far blur distance to [0,1] range
        fDepthBlur = ( fDepth - g_fFocalPlaneDistance ) / ( g_fFarBlurPlaneDistance - g_fFocalPlaneDistance );

        // Clamp the far blur to a maximum bluriness
        fDepthBlur = clamp( fDepthBlur, 0, g_fFarBlurLimit );
    }

    // Scale and bias the depth blur into the [0,1] range
    return clamp(fDepthBlur * 0.5f + 0.5f, 0, 1);
}
    

// fx doesn't support variable length arrays
// otherwise we could generalize this
half4 PS_Blur7(VS_OUTPUT_BLUR IN,
			   uniform sampler2D tex,
			   uniform half weight[7]
			   ) : COLOR
{
    half4 c = 0;
    
	c += tex2D(tex, IN.TexCoord0) * weight[0];
   	c += tex2D(tex, IN.TexCoord12.xy) * weight[1];
   	c += tex2D(tex, IN.TexCoord12.zw) * weight[2];
   	c += tex2D(tex, IN.TexCoord34.xy) * weight[3];
   	c += tex2D(tex, IN.TexCoord34.zw) * weight[4];
   	c += tex2D(tex, IN.TexCoord56.xy) * weight[5];
   	c += tex2D(tex, IN.TexCoord56.zw) * weight[6];
   	
    return c;
} 


half4 PS_Display(VS_OUTPUT IN,
			  uniform sampler2D tex) : COLOR
{   
	//return half4(tex2D(tex, IN.TexCoord0).xyz / 4, 1);
	return tex2D(tex, IN.TexCoord0);
}

half4 PS_Comp(VS_OUTPUT IN,
			  uniform sampler2D sceneSampler,
			  uniform sampler2D blurredSceneSampler,
			  uniform sampler2D depthSampler) : COLOR
{   

	float2 vPixelSizeHigh = 1.0 / WindowSize;
	float2 vPixelSizeLow = vPixelSizeHigh;

#if 0 // simple 

	float4 vTapLow   = tex2D( blurredSceneSampler,  IN.TexCoord0 );
    float4 vTapHigh   = tex2D( sceneSampler, IN.TexCoord0 );
   	vTapLow.a = ComputeDepthBlur( vTapLow.a );
	vTapHigh.a = ComputeDepthBlur( tex2D( depthSampler, IN.TexCoord0 ).r ); // 0..1
	float fTapBlur = abs( (vTapHigh.a * 2) - 1 ); // -1..0..1, to 0..1
	float4 vTap = lerp( vTapHigh, vTapLow, fTapBlur );
    
	// Normalize and return result
    return float4( vTap.rgb,1);

#else

	// Save depth
    float fCenterDepth =  tex2D( depthSampler, IN.TexCoord0 ).r;

    // Convert depth into blur radius in pixels
    float fDiscRadius = abs( fCenterDepth * g_vMaxCoC.y - g_vMaxCoC.x );
    fDiscRadius = clamp(fDiscRadius, 0, g_fMaxDiscRadius);
	
    // Compute disc radius on low-res image
    float fDiscRadiusLow = fDiscRadius * g_fRadiusScale;
    
    // Accumulate output color across all taps
    float4 vOutColor = 0;
    
    for( int t=0; t<NUM_POISSON_TAPS; t++ )
	{
        // Fetch lo-res tap
        float2 vCoordLow =  IN.TexCoord0 + (vPixelSizeLow * g_Poisson[t] * fDiscRadiusLow );
        float4 vTapLow   = tex2D( blurredSceneSampler, vCoordLow );
        
        // Fetch hi-res tap
        float2 vCoordHigh =  IN.TexCoord0 + (vPixelSizeHigh * g_Poisson[t] * fDiscRadius );
        float4 vTapHigh   = tex2D( sceneSampler, vCoordHigh );
        
	 	vTapLow.a = ComputeDepthBlur( vTapLow.a );
		vTapHigh.a = ComputeDepthBlur( tex2D( depthSampler, IN.TexCoord0 ).r ); 
	
        // Put tap bluriness into [1,..0..,1] range
        float fTapBlur = abs( (vTapHigh.a * 2.0f) - 1.0f );
        
        // Mix lo-res and hi-res taps based on bluriness
        float4 vTap = lerp( vTapHigh, vTapLow, fTapBlur );
        
        // Apply leaking reduction: lower weight for taps that are closer than the
        // center tap and in focus
        vTap.a = ( vTap.a >= fCenterDepth ) ? 1.0f : abs( vTap.a * 2.0001f - 1.0f );
		vTap.a = clamp( vTap.a, 0.001f, 1 ); // dx10 can get to zero causing divide by zero if all 0
  
        // Accumumate
        vOutColor.rgb += vTap.rgb * vTap.a;
        vOutColor.a   += vTap.a;
    }
    // Normalize and return result
	float4 c = vOutColor / vOutColor.a;
    return c;

#endif
}  



technique DOF
<
	string Script = "";
>
{
    pass BlurH
    <
    	string Script = "RenderColorTarget0=HBlurMap;"
	        			"Draw=Buffer;";
	>
    {
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_2_0 VS_Blur(7, float2(1, 0));
		PixelShader  = compile ps_2_0 PS_Blur7(SceneSampler, weights7);
	}
	pass BlurV
	<
		string Script = "RenderColorTarget0=FinalBlurMap;"
	        			"Draw=Buffer;";
	>
	{
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_2_0 VS_Blur(7, float2(0, 1));
		PixelShader  = compile ps_2_0 PS_Blur7(HBlurSampler, weights7);
	}
	pass FinalComp
	<
		string Script = "RenderColorTarget0=;"
						"Draw=Buffer;";        	
	>
	{
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_3_0 VS_Quad();
		PixelShader  = compile ps_3_0 PS_Comp(SceneSampler, FinalBlurSampler, ViewDepthMapSampler);
	}
}

#ifdef HKG_DX10

RasterizerState DisableCulling
{
    CullMode = NONE;
	MultisampleEnable = FALSE;
};

DepthStencilState DepthEnabling
{
	DepthEnable = FALSE;
	DepthWriteMask = ZERO;
};

BlendState DisableBlend
{
	BlendEnable[0] = FALSE;
	BlendEnable[1] = FALSE;
};

technique10 DOF10
<
	string Script = 
			"ClearColor0=ClearColor0;"
	        "ClearColor1=ClearColor1;"
	        "Clear=WipeAll;";
>
{
 
    pass BlurH
    <
    	string Script = "RenderColorTarget0=HBlurMap;"
	        			"Draw=Buffer;";
	>
    {

		SetVertexShader( CompileShader( vs_4_0, VS_Blur(7, float2(1, 0)) ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_Blur7(SceneSampler, weights7) ) );
		
		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
	}

	pass BlurV
	<
		string Script = "RenderColorTarget0=FinalBlurMap;"
	        			"Draw=Buffer;";
	>
	{
		SetVertexShader( CompileShader( vs_4_0, VS_Blur(7, float2(0, 1)) ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0,  PS_Blur7(HBlurSampler, weights7) ));

		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
	}

	pass FinalComp
	<
		string Script = "RenderColorTarget0=;"
						"Draw=Buffer;";        	
	>
	{
		SetVertexShader( CompileShader( vs_4_0, VS_Quad() ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_Comp(SceneSampler, FinalBlurSampler, ViewDepthMapSampler)) );
		//SetPixelShader( CompileShader( ps_4_0, PS_Display(ViewDepthMapSampler)) );

		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
  
	}
}

#ifdef HKG_DX11

technique11 DOF11
<
	string Script = 
			"ClearColor0=ClearColor0;"
	        "ClearColor1=ClearColor1;"
	        "Clear=WipeAll;";
>
{
    pass BlurH
    <
    	string Script = "RenderColorTarget0=HBlurMap;"
	        			"Draw=Buffer;";
	>
    {

		SetVertexShader( CompileShader( vs_4_0, VS_Blur(7, float2(1, 0)) ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_Blur7(SceneSampler, weights7) ) );
		
		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
	}

	pass BlurV
	<
		string Script = "RenderColorTarget0=FinalBlurMap;"
	        			"Draw=Buffer;";
	>
	{
		SetVertexShader( CompileShader( vs_4_0, VS_Blur(7, float2(0, 1)) ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0,  PS_Blur7(HBlurSampler, weights7) ));

		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
	}

	pass FinalComp
	<
		string Script = "RenderColorTarget0=;"
						"Draw=Buffer;";        	
	>
	{
		SetVertexShader( CompileShader( vs_4_0, VS_Quad() ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_Comp(SceneSampler, FinalBlurSampler, ViewDepthMapSampler)) );

		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
  
	}
}

#endif
#endif

