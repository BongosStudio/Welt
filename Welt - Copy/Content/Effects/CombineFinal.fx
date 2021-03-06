﻿float2 HalfPixel;

texture ColorMap;
sampler ColorSampler = sampler_state
{
	Texture = (ColorMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	Mipfilter = LINEAR;
};

texture LightMap;
sampler LightSampler = sampler_state
{
	Texture = (LightMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	Mipfilter = LINEAR;
};

struct VertexShaderInput
{
	float3 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.Position = float4(input.Position, 1);
	output.TexCoord = input.TexCoord - HalfPixel;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 diffuseColor = tex2D(ColorSampler, input.TexCoord).rgb;
	float4 light = tex2D(LightSampler, input.TexCoord);
	float3 diffuseLight = light.rgb;
	float specularLight = light.a;

	return float4((diffuseColor * diffuseLight + specularLight), 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}