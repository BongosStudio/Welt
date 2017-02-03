matrix WorldViewProj;

struct VertexShaderOut
{
	float4 Position : POSITION;
	float Depth : TEXCOORD0;
};

VertexShaderOut ShadowMapVertexShader(float4 position : SV_POSITION)
{
	VertexShaderOut output;
	output.Position = mul(position, WorldViewProj);
	output.Depth = output.Position.z / output.Position.w;
	return output;
}

float4 ShadowMapPixelShader(VertexShaderOut input) : COLOR
{
	return float4(input.Depth, 0, 0, 0);
}

technique CreateShadowMap
{
	pass Pass1
	{
		VertexShader = compile vs_5_0 ShadowMapVertexShader();
		PixelShader = compile ps_5_0 ShadowMapPixelShader();
	}
}