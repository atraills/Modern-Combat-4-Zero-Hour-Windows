// Simple color correction using a 3D LUT
// Use GenColorLUT in Demo/Graphics/Tools to create a custom LUT

float Script : STANDARDSGLOBAL
<
	string UIWidget = "none";
	string ScriptClass = "scene";
	string ScriptOrder = "postprocess";
	string ScriptOutput = "color";
	
	// We just call a script in the main technique.
#ifdef HKG_DX11
	string Script = "Technique=LUT11;";
#elif defined(HKG_DX10)
	string Script = "Technique=LUT10;";
#else
	string Script = "Technique=LUT;";
#endif
> = 1.0;

float4 WindowSize : VIEWPORTPIXELSIZE;

texture SceneMap : RENDERCOLORTARGET
<
	string source = "SCENE";
>;

#ifdef SUPPORT_HDR
float4 ColorRangeCorrection  : COLORRANGE;
float4 colorRangeCorrect( float4 rgba )
{
	float3 c0 = (( 1 - ColorRangeCorrection[0] ) * rgba.rgb );
	float3 c1 =  ColorRangeCorrection[0] * pow( (ColorRangeCorrection[2] * rgba.rgb ), ColorRangeCorrection[1] ); // Exposure and Gamma
	return float4(c0 + c1, rgba.a);
}
#endif

sampler SceneSampler = sampler_state 
{
    texture = <SceneMap>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
	#ifdef HKG_DX10
	  FILTER = MIN_MAG_MIP_POINT;
	#else
       MIPFILTER = NONE;
       MINFILTER = POINT;
       MAGFILTER = POINT;
	#endif
};

texture LutMap
<
	string source = "USER";
>;

sampler LutSampler = sampler_state 
{
    texture = <LutMap>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
	#ifdef HKG_DX10
	  FILTER = MIN_MAG_LINEAR_MIP_POINT;
	#else
       MIPFILTER = NONE;
       MINFILTER = LINEAR;
       MAGFILTER = LINEAR;
	#endif
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
	OUT.Position.xy += (dxHalfTexelPos * 0.5);
#endif
	
	OUT.TexCoord0 = TexCoord;
 	
	return OUT;
}


half4 PS_Color(VS_OUTPUT IN,
			  uniform sampler2D sceneSampler,
			  uniform sampler3D lutSampler) : COLOR
{   
	half4 orig = tex2D(sceneSampler, IN.TexCoord0);
#ifdef SUPPORT_HDR
	orig = colorRangeCorrect(orig); // adjust for gamma, exposure etc before LUT?
#endif
	half4 corrected = half4( tex3D(lutSampler, orig.rgb ).rgb , 1);
	
	
	return corrected;
}  



technique LUT
<
	string Script = "";
>
{
 	pass ColorCorrect
	<
		string Script = "RenderColorTarget0=;"
						"Draw=Buffer;";        	
	>
	{
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_2_0 VS_Quad();
		PixelShader  = compile ps_2_0 PS_Color(SceneSampler, LutSampler);
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

#ifdef HKG_DX11
technique10 LUT11
#else
technique10 LUT10
#endif
<
	string Script = "";
>
{
	pass ColorCorrect
	<
		string Script = "RenderColorTarget0=;"
						"Draw=Buffer;";        	
	>
	{
		SetVertexShader( CompileShader( vs_4_0, VS_Quad() ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS_Color(SceneSampler, LutSampler)) );

		SetRasterizerState(DisableCulling);       
		SetDepthStencilState(DepthEnabling, 0);
		SetBlendState(DisableBlend, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF);
  
	}
}

#endif