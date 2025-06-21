
float Script : STANDARDSGLOBAL
<
	string UIWidget = "none";
	string ScriptClass = "scene";
	string ScriptOrder = "postprocess";
	string ScriptOutput = "color";
	
	// We just call a script in the main technique.
#ifdef HKG_DX11
	string Script = "Technique=MotionBlur11;";
#elif defined(HKG_DX10)
	string Script = "Technique=MotionBlur10;";
#else
	string Script = "Technique=MotionBlur;";
#endif
> = 0.8;


float SceneIntensity <
    string UIName = "Scene intensity";
    string UIWidget = "slider";
    float UIMin = 0.0f;
    float UIMax = 2.0f;
    float UIStep = 0.1f;
> = 1.0f;

float2 WindowSize : VIEWPORTPIXELSIZE < string UIWidget = "none"; >;
float PixelBlurConst = 1.0f;
static const int NumberOfPostProcessSamples = 12;
float VelocityClamp = 0.05f;

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
//   MIPFILTER = NONE;
//    MINFILTER = POINT;
//    MAGFILTER = POINT;
};

texture VelocityMap : RENDERCOLORTARGET
< 
	string source = "VELOCITY";
>;

sampler VelocityMapSampler = sampler_state 
{
    texture = <VelocityMap>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
 	FILTER = MIN_MAG_LINEAR_MIP_POINT;
//   MIPFILTER = NONE;
//    MINFILTER = LINEAR;
//    MAGFILTER = LINEAR;
};

struct VS_OUTPUT
{
   	float4 Position   : POSITION;
    float2 TexCoord0  : TEXCOORD0;
};

VS_OUTPUT VS_Quad(float4 Position : POSITION, 
				  float2 TexCoord : TEXCOORD0)
{
    VS_OUTPUT OUT;
	OUT.Position = Position; 

	float2 texelSize = 1.0 / WindowSize;

#ifndef HKG_DX10
	float2 dxHalfTexelPos = float2( -texelSize.x, texelSize.y ); // [-1,1] -> WindowSize  ==  2/WindowSize,  so half texel = 1/windowize
	OUT.Position.xy += (dxHalfTexelPos * 1);
#endif
	
	OUT.TexCoord0 = TexCoord;
    
	return OUT;
}

float4 PS_Display(VS_OUTPUT IN,
			  uniform sampler2D tex) : COLOR
{   
	return tex2D(tex, IN.TexCoord0);
}

float4 PS_Comp(VS_OUTPUT IN,
			  uniform sampler2D sceneSampler,
			  uniform sampler2D velSampler) : COLOR
{   
	float4 orig = tex2D(sceneSampler, IN.TexCoord0);
	float4 vel = tex2D(velSampler, IN.TexCoord0);
	
	return SceneIntensity*orig + (1-SceneIntensity)*(float4(vel.r, vel.g, 0, 1));
}  

float4 PS_MotionBlur(VS_OUTPUT IN,
			  uniform sampler2D sceneSampler,
			  uniform sampler2D velSampler) : COLOR
{
	
	float2 pixelVelocity;
    float4 curFramePixelVelocity = tex2D(velSampler, IN.TexCoord0);
    //float4 lastFramePixelVelocity = tex2D(lastVelSampler, IN.TexCoord0);

    pixelVelocity.x =  -curFramePixelVelocity.r * PixelBlurConst;   
    pixelVelocity.y =  curFramePixelVelocity.g * PixelBlurConst;    
    pixelVelocity = clamp(pixelVelocity, -VelocityClamp, VelocityClamp);
    float3 BlurredTotal = 0;    
    for(int i = 0; i < NumberOfPostProcessSamples; i++)
    {   
        float2 lookup = pixelVelocity * i / NumberOfPostProcessSamples + IN.TexCoord0;
        float4 Current = tex2D(sceneSampler, lookup);
        BlurredTotal += Current.rgb;
    }
    
    // Return the average color of all the samples
    return float4(BlurredTotal / NumberOfPostProcessSamples, 1.0f);
}

technique MotionBlur
<
	string Script = "";
>
{
	pass VelocityBasedBlur
	<
		string Script = "RenderColorTarget0=;"
						"Draw=Buffer;";        	
	>
	{
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_2_0 VS_Quad();
		PixelShader  = compile ps_2_0 PS_MotionBlur(SceneSampler, VelocityMapSampler);
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
};

technique10 MotionBlur10
<
	string Script = "";
>
{
	pass VelocityBasedBlur
	<
		string Script = "RenderColorTarget0=;"
						"Draw=Buffer;";        	
	>
	{
		SetVertexShader( CompileShader( vs_4_0, VS_Quad() ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_MotionBlur(SceneSampler, VelocityMapSampler)) );

		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
  
	}
}

#ifdef HKG_DX11

technique11 MotionBlur11
<
	string Script = "";
>
{
	pass VelocityBasedBlur
	<
		string Script = "RenderColorTarget0=;"
						"Draw=Buffer;";        	
	>
	{
		SetVertexShader( CompileShader( vs_4_0, VS_Quad() ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_MotionBlur(SceneSampler, VelocityMapSampler)) );

		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
  
	}
}

#endif
#endif
