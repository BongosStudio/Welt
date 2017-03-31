float3 LightDirection;
float3 LightColor;
float3 CameraPosition;
float4x4 InvertViewProjection;

float2 HalfPixel;

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

texture TextureMap;
sampler TextureSampler = sampler_state
{
	Texture = (TextureMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	Mipfilter = LINEAR;
};

texture DepthMap;
sampler DepthSampler = sampler_state
{
	Texture = (DepthMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = POINT;
	MinFilter = POINT;
	Mipfilter = POINT;
};

texture NormalMap;
sampler NormalSampler = sampler_state
{
	Texture = (NormalMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = POINT;
	MinFilter = POINT;
	Mipfilter = POINT;
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
	//align texture coordinates
	output.TexCoord = input.TexCoord - HalfPixel;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//get normal data from the normalMap
	float4 normalData = tex2D(NormalSampler, input.TexCoord);
	//tranform normal back into [-1,1] range
	float3 normal = 2.0f * normalData.xyz - 1.0f;
	//get specular power, and get it into [0,255] range]
	float specularPower = normalData.a * 255;
	//get specular intensity from the colorMap
	float specularIntensity = tex2D(ColorSampler, input.TexCoord).a;

	//read depth
	float depthVal = tex2D(DepthSampler, input.TexCoord).r;

	//compute screen-space position
	float4 position;
	position.x = input.TexCoord.x * 2.0f - 1.0f;
	position.y = -(input.TexCoord.x * 2.0f - 1.0f);
	position.z = depthVal;
	position.w = 1.0f;
	//transform to world space
	position = mul(position, InvertViewProjection);
	position /= position.w;

	//surface-to-light vector
	float3 lightVector = -normalize(LightDirection);

	//compute diffuse light
	float NdL = max(0, dot(normal, lightVector));
	float3 diffuseLight = NdL * LightColor.rgb;

	//reflexion vector
	float3 reflectionVector = normalize(reflect(-lightVector, normal));
	//camera-to-surface vector
	float3 directionToCamera = normalize(CameraPosition - position);
	//compute specular light
	float specularLight = specularIntensity * pow(saturate(dot(reflectionVector, directionToCamera)), specularPower);

	//output the two lights
	return float4(diffuseLight.rgb, specularLight);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}