float4x4 World;
float4x4 View;
float4x4 Projection;

float3 CameraPosition;

float FogNear = 250;
float FogFar = 300;
float SpecularPower = 0.5f;
float SpecularIntensity = 0.8f;
//float4 FogColor;
//float3 SunColor;
float TimeOfDay;

float4 HorizonColor;
float4 SunColor;
float4 NightColor;
float4 DeepWaterColor;

float4 MorningTint;
float4 EveningTint;

float3 HeldItemLight;
bool HasFlicker;

float3 SkyLightPosition; 

bool IsUnderWater;
float RippleTime;
float Random;

/* Effect IDs */
const int NONE = 0;
const int LightLiquidEffect = 1; // will apply ripples and vertex distortion
const int HeavyLiquidEffect = 2; // will apply slow ripples and short range visibility
const int VegetationWindEffect = 3; // will apply a "sway in the breeze" effect

float ScreenWidth;
float ScreenHeight;

float4	g_vSine9 = float4(-0.16161616f, 0.0083333f, -0.00019841f, 0.000002755731f);
float4	g_fFixupFactor = 1.07f;
float4	g_vWaveDistortX = float4(3.0f, 0.4f, 0.0f, 0.3f);
float4	g_vWaveDistortY = float4(3.0f, 0.4f, 0.0f, 0.3f);
float4	g_vWaveDistortZ = float4(-1.0f, -0.133f, -0.333f, -0.10f);
float4	g_vWaveDirX = float4(-0.006f, -0.012f, 0.024f, 0.048f);
float4	g_vWaveDirY = float4(-0.003f, -0.006f, -0.012f, -0.048f);
float4	g_vWaveSpeed = float4(0.3f, 0.6f, 0.7f, 1.4f);
float	g_fPIx2 = 6.28318530f;
float4	g_vLightingWaveScale = float4(0.35f, 0.10f, 0.10f, 0.03f);
float4	g_vLightingScaleBias = float4(0.6f, 0.7f, 0.2f, 0.0f);

texture Texture1;
sampler Texture1Sampler = sampler_state
{
	texture = <Texture1>;
	magfilter = POINT;
	minfilter = POINT;
	mipfilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};

texture ColorTexture;
sampler ColorMap = sampler_state
{
	Texture = <ColorTexture>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 TexCoords1 : TEXCOORD0;
	uint Effect : BLENDWEIGHT0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 TexCoords1 : TEXCOORD0;
	float Distance : TEXCOORD1;
	float2 Depth : TEXCOORD2;
	float Effect : BLENDWEIGHT1;
};

struct PixelShaderOutput
{
	half4 Color : COLOR0;
	half4 Normal : COLOR1;
	half4 Depth : COLOR2;
};

int LFSR_Rand_Gen(int n)
{
	// <<, ^ and & require GL_EXT_gpu_shader4.
	n = (n << 13) ^ n;
	return (n * (n*n * 15731 + 789221) + 1376312589) & 0x7fffffff;
}

float LFSR_Rand_Gen_f(int n)
{
	return float(LFSR_Rand_Gen(n));
}

float noise3f(float3 p)
{
	float3 ip = float3(floor(p));
	float3 u = frac(p);
	u = u*u*(3.0 - 2.0*u);

	int n = ip.x + ip.y * 57 + ip.z * 113;

	float res = lerp(lerp(lerp(LFSR_Rand_Gen_f(n + (0 + 57 * 0 + 113 * 0)),
		LFSR_Rand_Gen_f(n + (1 + 57 * 0 + 113 * 0)), u.x),
		lerp(LFSR_Rand_Gen_f(n + (0 + 57 * 1 + 113 * 0)),
			LFSR_Rand_Gen_f(n + (1 + 57 * 1 + 113 * 0)), u.x), u.y),
		lerp(lerp(LFSR_Rand_Gen_f(n + (0 + 57 * 0 + 113 * 1)),
			LFSR_Rand_Gen_f(n + (1 + 57 * 0 + 113 * 1)), u.x),
			lerp(LFSR_Rand_Gen_f(n + (0 + 57 * 1 + 113 * 1)),
				LFSR_Rand_Gen_f(n + (1 + 57 * 1 + 113 * 1)), u.x), u.y), u.z);

	return 1.0 - res*(1.0 / 1073741824.0);
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	input.Position.w = 1.0f;

	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, World);
	output.Depth.x = output.Position.z;
	output.Depth.y = output.Position.w;
	output.Distance = length(CameraPosition - worldPosition);
	output.TexCoords1 = input.TexCoords1;

	float4 sColor = SunColor;

	if (TimeOfDay <= 12)
	{
		sColor *= TimeOfDay / 12;
	}
	else
	{
		sColor *= (TimeOfDay - 24) / -12;
	}

	switch (input.Effect)
	{
	case 1: // LightLiquidEffect
		output.Position.y += (0.1f * sin(RippleTime + input.Position.x + (input.Position.z * 2))) - 0.2f;
		// TODO: display reflection
		break;
	case 3: // grass wave effect
		float n = noise3f(input.Position.xyz);
		output.Position.x += (0.05f * sin((RippleTime/2)*n*2 + input.Position.x + (input.Position.z * 2)));
		break;
	default:
		break;
	}
	//output.Color.xyz += (input.Overlay.rgb * input.Overlay.a) * input.SunLight;

	if (IsUnderWater)
	{
		output.Position.x += (0.05f * sin(RippleTime + input.Position.x + (input.Position.z * 2))) - 0.2f;
	}

	// gives a rounded shape to the landscape, making it seem like an actual planet.
	// this value needs to be dependant on world size so smaller planets have a larger bend.
	output.Position.y -= output.Distance * output.Distance *0.001f;
	output.Effect = input.Effect;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	PixelShaderOutput output;

	float4 texColor1 = tex2D(Texture1Sampler, input.TexCoords1);
	float fog = saturate((input.Distance - FogNear) / (FogNear - FogFar));
	float4 topColor = SunColor;
	float4 FogColor = HorizonColor;
	float4 nColor = NightColor;
	float4 color;
	float4 outputFogColor;
	color.rgb = texColor1.rgb;
	if (IsUnderWater)
	{
		float vis = saturate((input.Distance - 50) / -25);
		color = lerp(DeepWaterColor, color, vis);
		color.r /= 2;
		color.g /= 1.5;
		
	}
	color.a = texColor1.a;
	
	nColor *= (4 - input.TexCoords1.y) * .125f;

	if (TimeOfDay <= 12)
	{
		topColor *= TimeOfDay / 12;
		FogColor *= TimeOfDay / 12;
		nColor *= TimeOfDay / 12;
	}
	else
	{
		FogColor *= (TimeOfDay - 24) / -12;
		topColor *= (TimeOfDay - 24) / -12;
		nColor *= (TimeOfDay - 24) / -12;
	}

	FogColor += (MorningTint * .05) * ((24 - TimeOfDay) / 24);
	FogColor += (EveningTint * .05) * (TimeOfDay / 24);
	topColor += nColor;
	FogColor += nColor;

	outputFogColor = lerp(FogColor, topColor, saturate((input.TexCoords1.y) / 0.9f));

	output.Color = lerp(FogColor, color, fog);
	output.Color.a = SpecularIntensity;
	output.Normal.rgb = 0.5f * (normalize(input.Normal) + 1.0f);
	output.Normal.a = SpecularPower;
	output.Depth = input.Depth.x / input.Depth.y;
	return color;
	//return output.Color;
}

technique BlockTechnique
{
	pass Pass1
	{
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}