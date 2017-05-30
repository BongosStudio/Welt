float4x4 WorldViewProjection;
float4x4 InvViewProjection;

float ScreenWidth;
float ScreenHeight;

float3 LightColor;
float3 LightPosition;
float LightAttenuation;

texture2D DepthTexture;
sampler2D DepthTextureSampler = sampler_state
{
    texture = <DepthTexture>;
    minfilter = POINT;
    magfilter = POINT;
    mipfilter = POINT;
};

texture2D NormalTexture;
sampler2D NormalTextureSampler = sampler_state
{
    texture = <NormalTexture>;
    minfilter = POINT;
    magfilter = POINT;
    mipfilter = POINT;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 LightPosition : TEXCOORD0;
};

float2 PostProjToScreen(float4 position)
{
    float2 screenPosition = position.xy / position.w;

    return 0.5f * (float2(screenPosition.x, -screenPosition.y) + 1);
}

float HalfPixel()
{
    return 0.5f / float2(ScreenWidth, ScreenHeight);
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = mul(input.Position, WorldViewProjection);
    output.LightPosition = output.Position;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float2 texCoords = PostProjToScreen(input.LightPosition) + HalfPixel();
    float4 depth = tex2D(DepthTextureSampler, texCoords);

    float4 position = texCoords.x * 2 - 1;
    position.y = (1 - texCoords.y) * 2 - 1;
    position.z = depth.r;
    position.w = 1.0f;
    position = mul(position, InvViewProjection);
    position.xyz /= position.w;
    float4 normal = (tex2D(NormalTextureSampler, texCoords) - 0.5) * 2;
    float3 lightDirection = normalize(LightPosition - position);
    float lighting = clamp(dot(normal, lightDirection), 0, 1);

    float d = distance(LightPosition, position);
    float att = 1 - pow(d / LightAttenuation, 6);
    return float4(LightColor * lighting * att, 1);
}

technique Tech
{
    pass Pass1
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}