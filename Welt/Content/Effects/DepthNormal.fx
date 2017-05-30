float4x4 World;
float4x4 View;
float4x4 Projection;

float3 CameraPosition;

float ScreenWidth;
float ScreenHeight;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoords1 : TEXCOORD0;
    float Effect : BLENDWEIGHT0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 Depth : TEXCOORD0;
    float3 Normal : TEXCOORD1;
};

struct PixelShaderOutput
{
    float4 Normal : COLOR0;
    float4 Depth : COLOR1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    input.Position.w = 1.0f;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.Normal = input.Normal;
    output.Depth.xy = output.Position.zw;
    
    return output;
}

PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
{
    PixelShaderOutput output;
    
    output.Depth = input.Depth.x / input.Depth.y;
    output.Normal.xyz = (normalize(input.Normal).xyz / 2) + 0.5;
    output.Depth.a = 1;
    output.Normal.a = 1;
	
    return output;
}

technique Tech
{
    pass Pass1
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}