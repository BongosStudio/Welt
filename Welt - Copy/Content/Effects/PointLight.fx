float4x4 World;
float4x4 Projection;
float4x4 View;

float3 CameraPosition;
float4x4 InvertViewProjection;

float2 HalfPixel;

float3 LightColor;
float3 LightPosition;
float LightRadius;
float LightIntensity = 1.0f;
texture ColorMap;
texture NormalMap;
texture DepthMap;

sampler ColorSampler = sampler_state
{
	Texture = (ColorMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	Mipfilter = LINEAR;
};

sampler DepthSampler = sampler_state
{
	Texture = (DepthMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = POINT;
	MinFilter = POINT;
	Mipfilter = POINT;
};
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
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 ScreenPosition : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	//processing geometry coordinates
	float4 worldPosition = mul(float4(input.Position, 1), World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.ScreenPosition = output.Position;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	input.ScreenPosition.xy /= input.ScreenPosition.w;

	float2 texCoord = 0.5f * (float2(input.ScreenPosition.x,-input.ScreenPosition.y) + 1);
	texCoord -= HalfPixel;

	float4 normalData = tex2D(NormalSampler,texCoord);
	float3 normal = 2.0f * normalData.xyz - 1.0f;
	float specularPower = normalData.a * 255;
	float specularIntensity = tex2D(ColorSampler, texCoord).a;
	float depthVal = tex2D(DepthSampler,texCoord).r;
	float4 position;

	position.xy = input.ScreenPosition.xy;
	position.z = depthVal;
	position.w = 1.0f;
	position = mul(position, InvertViewProjection);
	position /= position.w;

	float3 lightVector = LightPosition - position;
	float attenuation = saturate(1.0f - length(lightVector) / LightRadius);

	lightVector = normalize(lightVector);

	float NdL = max(0,dot(normal, lightVector));
	float3 diffuseLight = NdL * LightColor.rgb;
	float3 reflectionVector = normalize(reflect(-lightVector, normal));
	float3 directionToCamera = normalize(CameraPosition - position);
	float specularLight = specularIntensity * pow(saturate(dot(reflectionVector, directionToCamera)), specularPower);

	return attenuation * LightIntensity * float4(diffuseLight.rgb,specularLight);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}