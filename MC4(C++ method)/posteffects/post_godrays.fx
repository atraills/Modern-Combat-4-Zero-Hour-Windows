// Crytek style : radial blur the depth mask (1 - depth) and use that as as the basis for the God rays 

float4 ClearColor0 : DIFFUSE = { 0.0f, 0.0f, 0.0f, 1.0f};
float4 ClearColor1= { 10000.0f, 0.0f, 0.0f, 0.0f};
float ClearDepth = 1.0f;

float Script : STANDARDSGLOBAL
<
#ifdef HKG_DX11
	string Script = "Technique=GodRays11;";
#elif defined(HKG_DX10)
	string Script = "Technique=GodRays10;";
#else
	string Script = "Technique=GodRays;";
#endif
>;

float4 WindowSize : VIEWPORTPIXELSIZE;
float BlurStart; //1 pixel say
float BlurWidth; //-0.2 say
float2 LightPosScreenSpace;
float g_effectScale;
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


texture DepthRadialBlurMapA : RENDERCOLORTARGET
< 
    float2 ViewportRatio = { 0.25, 0.25 };
    int MIPLEVELS = 1;
    string format = "R32F";
>;

texture DepthRadialBlurMapB : RENDERCOLORTARGET
< 
    float2 ViewportRatio = { 0.25, 0.25 };
    int MIPLEVELS = 1;
    string format = "R32F";
>;

sampler DepthRadialBlurSamplerA = sampler_state 
{
    texture = <DepthRadialBlurMapA>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
	FILTER = MIN_MAG_LINEAR_MIP_POINT;
};

sampler DepthRadialBlurSamplerB = sampler_state 
{
    texture = <DepthRadialBlurMapB>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
	FILTER = MIN_MAG_LINEAR_MIP_POINT;
};



struct VS_OUTPUT_BLUR
{
    float4 Position   : SV_Position;
    float2 TexCoord0 : TEXCOORD0;
};

struct VS_OUTPUT
{
   	float4 Position   : SV_Position;
    float2 TexCoord0  : TEXCOORD0;
};


// generate texcoords for blur
VS_OUTPUT_BLUR VS_RadialBlur(float4 Position : POSITION, 
					   float2 TexCoord : TEXCOORD0,
					   uniform int nsamples 
					  )
{
    VS_OUTPUT_BLUR OUT = (VS_OUTPUT_BLUR)0;

	OUT.Position = Position;
	
	float2 texelSize = 1.0 / WindowSize.xy;
	
#ifndef HKG_DX10
	float2 dxHalfTexelPos = float2( -texelSize.x, texelSize.y ); // [-1,1] -> WindowSize  ==  2/WindowSize,  so half texel = 1/windowize
	OUT.Position.xy += (dxHalfTexelPos * Position.w);
#endif

	OUT.TexCoord0 = TexCoord - LightPosScreenSpace;
    
    return OUT;
}

VS_OUTPUT VS_Quad(float4 Position : POSITION, 
				  float2 TexCoord : TEXCOORD0)
{
    VS_OUTPUT OUT;
	OUT.Position = Position; 

	
	float2 texelSize = 1.0 / WindowSize.xy;
	
#ifndef HKG_DX10
	
	float2 dxHalfTexelPos = float2( -texelSize.x, texelSize.y ); // [-1,1] -> WindowSize  ==  2/WindowSize,  so half texel = 1/windowize
	OUT.Position.xy += (dxHalfTexelPos * Position.w);
#endif
	
	OUT.TexCoord0 = TexCoord;
 	
	return OUT;
}

// fx doesn't support variable length arrays
// otherwise we could generalize this
half4 PS_RadialBlur(VS_OUTPUT_BLUR IN,
			   uniform sampler2D tex,
			   uniform int nsamples,
			   uniform int passNum
			   ) : COLOR
{
    float2 Center = LightPosScreenSpace;
    half4 c = 0;
    
    for(int i=0; i<nsamples; i++) 
    {
    	float scale = BlurStart + (BlurWidth/passNum)*(i / (float)(nsamples-1.0));
    	c += tex2D(tex, IN.TexCoord0.xy*scale + Center );
    }
    
    c /= nsamples;
    
    return c;
} 


half4 PS_Display(VS_OUTPUT IN,
			  uniform sampler2D tex) : COLOR
{   
	return 1 - tex2D(tex, IN.TexCoord0).r;
}


half4 PS_GodRays(VS_OUTPUT IN,
			  uniform sampler2D sceneSampler,
			  uniform sampler2D radialBluredDepthSampler
			  ) : COLOR
{   
	half4 scene = tex2D(sceneSampler, IN.TexCoord0);
	half shaft = saturate( 1.0f - tex2D(radialBluredDepthSampler, IN.TexCoord0).r );
	float shaftDecay = clamp( 200 / length(LightPosScreenSpace.xy - IN.Position.xy), 0, 0.25);
	return scene + half4(shaft, shaft, shaft, 0)*shaftDecay*g_effectScale;
}  

technique GodRays
<
	string Script = "ClearColor0=ClearColor0;"
					"ClearColor1=ClearColor1;"
					"Clear=WipeAll;";
>
{
    pass RadialBlur0
    <
    	string Script = "RenderColorTarget0=DepthRadialBlurMapA;"
	        			"Draw=Buffer;";
	>
    {
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_3_0 VS_RadialBlur(8);
		PixelShader  = compile ps_3_0 PS_RadialBlur(ViewDepthMapSampler, 8, 1);
	}
	pass RadialBlur1
	<
		string Script = "RenderColorTarget0=DepthRadialBlurMapB;"
	        			"Draw=Buffer;";
	>
	{
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_3_0 VS_RadialBlur(8);
		PixelShader  = compile ps_3_0 PS_RadialBlur(DepthRadialBlurSamplerA, 8, 2);
	}
	pass RadialBlur2
	<
		string Script = "RenderColorTarget0=DepthRadialBlurMapA;"
						"Draw=Buffer;";        	
	>
	{
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_3_0 VS_RadialBlur(8);
		PixelShader  = compile ps_3_0 PS_RadialBlur(DepthRadialBlurSamplerB, 8, 3);
	}
	pass 
	<
		string Script = "RenderColorTarget0=;"
						"Draw=Buffer;";        	
	>
	{
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_3_0 VS_Quad();
		//PixelShader  = compile ps_3_0 PS_Display(ViewDepthMapSampler);
		PixelShader  = compile ps_3_0 PS_GodRays(SceneSampler, DepthRadialBlurSamplerA);
	}
}

#ifdef HKG_DX10 // or 11

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

#ifdef HKG_DX11
technique11 GodRays11
#else
technique10 GodRays10
#endif
<
	string Script = "";
>
{
 
    pass RadialBlur0
    <
    	string Script = "RenderColorTarget0=DepthRadialBlurMapA;"
	        			"Draw=Buffer;";
	>
    {

		SetVertexShader( CompileShader( vs_4_0, VS_RadialBlur(8) ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_RadialBlur(ViewDepthMapSampler,8, 1) ) );
		
		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
	}

	pass RadialBlur1
    <
    	string Script = "RenderColorTarget0=DepthRadialBlurMapB;"
	        			"Draw=Buffer;";
	>
    {

		SetVertexShader( CompileShader( vs_4_0, VS_RadialBlur(8) ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_RadialBlur(DepthRadialBlurSamplerA,8, 2) ) );
		
		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
	}

	pass RadialBlur2
    <
    	string Script = "RenderColorTarget0=DepthRadialBlurMapA;"
	        			"Draw=Buffer;";
	>
    {

		SetVertexShader( CompileShader( vs_4_0, VS_RadialBlur(8) ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_RadialBlur(DepthRadialBlurSamplerB,8, 3) ) );
		
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
        SetPixelShader( CompileShader( ps_4_0, PS_GodRays(SceneSampler, DepthRadialBlurSamplerA)) );
		//SetPixelShader( CompileShader( ps_4_0, PS_Display(ViewDepthMapSampler)) );

		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
  
	}
}
#endif

