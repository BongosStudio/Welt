float4x4 World;
float4x4 View;
float4x4 Projection;

float3 CameraPosition;

float WaveTime;
Texture Texture0;

sampler Texture1Sampler = sampler_state {
	texture = <Texture0>;
	magfilter = POINT;
	minfilter = POINT;
	mipfilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};

struct VertexShaderInput {
	float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float SunLight : COLOR0;
	float3 LocalLight : COLOR1;
};

struct VertexShaderOutput {
	float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float3 CameraView : TEXCOORD1;
	float Distance : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input) {
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);

	output.Position = mul(viewPosition, Projection);
	output.CameraView = normalize(CameraPosition - worldPosition);
	output.Distance = length(CameraPosition - worldPosition);
	output.Position.x += (0.2f * sin(WaveTime + input.Position.z * 2)) - 0.2f;
	output.TexCoords = input.TexCoords;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 texColor1 = tex2D(Texture1Sampler, input.TexCoords);
	return texColor1;
}

technique BlockTechnique {
	pass Pass1 {
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}