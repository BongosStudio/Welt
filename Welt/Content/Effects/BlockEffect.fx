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
float4 SunPosition;

float4 MorningTint;
float4 EveningTint;

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

texture BlockTexture;
sampler BlockTextureSampler = sampler_state
{
	texture = <BlockTexture>;
	magfilter = POINT;
	minfilter = POINT;
	mipfilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};

texture ReflectionTexture;
sampler ReflectionTextureSampler = sampler_state
{
    texture = <ReflectionTexture>;
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
	float Effect : BLENDWEIGHT0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoords1 : TEXCOORD0;
	float3 CameraView : TEXCOORD1;
	float Distance : TEXCOORD2;
	float4 Color : COLOR0;
	float Effect : BLENDWEIGHT1;
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
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.CameraView = normalize(CameraPosition - worldPosition);
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
		output.Color.rgb = (sColor * 15);
		output.Color.a = 1;
		break;
	case 3: // grass wave effect
		float n = noise3f(input.Position.xyz);
		output.Position.x += (0.05f * sin((RippleTime/2)*n*2 + input.Position.x + (input.Position.z * 2)));
		/*if (HasFlicker)
		{
			output.Color.rgb = (sColor * (15 * saturate(length(SunPosition - input.Position)))) + (((HeldItemLight * 0.75 * (1 + sin(RippleTime)*(Random*0.1))) / output.Distance) * 0.25);
		}
		else
		{
			output.Color.rgb = (sColor * (15 * saturate(length(SunPosition - input.Position))));
		}*/
		output.Color.rgb = sColor * 15;
		output.Color.a = 0.3;
		break;
	default:
		/*if (HasFlicker)
		{
			output.Color.rgb = (sColor * (15 * saturate(length(SunPosition - input.Position)))) + (((HeldItemLight * 0.75 * (1 + sin(RippleTime)*(Random*0.1))) / output.Distance) * 0.25);
		}
		else
		{
			output.Color.rgb = (sColor * (15 * saturate(length(SunPosition - input.Position))));
		}*/
		output.Color.rgb = sColor * 15;
		output.Color.a = 1;
		break;
	}
	//output.Color.xyz += (input.Overlay.rgb * input.Overlay.a) * input.SunLight;

	output.Color.xyz = float3(1, 1, 1);

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
	float4 texColor1 = tex2D(BlockTextureSampler, input.TexCoords1);
	float fog = saturate((input.Distance - FogNear) / (FogNear - FogFar));
	float4 topColor = SunColor;
	float4 FogColor = HorizonColor;
	float4 nColor = NightColor;
	float4 color;
	float4 outputFogColor;

	color.rgb = texColor1.rgb * input.Color.rgb;
    //color.r /= saturate((input.Distance - 50) / -25);
    //color.b /= saturate((input.Distance - 50) / 25);
	if (IsUnderWater)
	{
		float vis = saturate((input.Distance - 50) / -25);
		color = lerp(DeepWaterColor, color, vis);
		color.r /= 2;
		color.g /= 1.5;

	}
	color.a = texColor1.a;
	switch (input.Effect)
	{
		case 1:
            float4 reflColor = tex2D(ReflectionTextureSampler, input.Position.xz);
            //color.rgb *= reflColor;
			break;
		default:
			if (color.a == 0)
			{
				clip(-1);
			}
			break;
	}

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

	return lerp(outputFogColor, color, fog);

}

technique BlockTechnique
{
	pass Pass1
	{
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}