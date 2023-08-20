Shader "Selected Effect --- Outline/Post Process/Func" {
	Properties {
		[HideInInspector]_MainTex ("Main", 2D) = "white" {}
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	float4 _Global_Offsets;
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	struct V2FBlur {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float4 uv01 : TEXCOORD1;
		float4 uv23 : TEXCOORD2;
		float4 uv45 : TEXCOORD3;
		float4 uv67 : TEXCOORD4; 
		float4 uv89 : TEXCOORD5;
	};
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	V2FBlur vertBlur (appdata_img v)
	{
		V2FBlur o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv.xy = v.texcoord.xy;
		o.uv01 =  v.texcoord.xyxy + _Global_Offsets.xyxy * float4(1,1, -1,-1) * (1 / _ScreenParams.xyxy);
		o.uv23 =  v.texcoord.xyxy + _Global_Offsets.xyxy * float4(2,2, -2,-2) * (1 / _ScreenParams.xyxy);
		o.uv45 =  v.texcoord.xyxy + _Global_Offsets.xyxy * float4(3,3, -3,-3) * (1 / _ScreenParams.xyxy);
		o.uv67 =  v.texcoord.xyxy + _Global_Offsets.xyxy * float4(4,4, -4,-4) * (1 / _ScreenParams.xyxy);
		o.uv89 =  v.texcoord.xyxy + _Global_Offsets.xyxy * float4(5,5, -5,-5) * (1 / _ScreenParams.xyxy);
		return o;
	}
	float4 fragBlur (V2FBlur input) : SV_Target
	{
		float sum = 0;
		float w = 0;
		float weights = 0;
		const float G_WEIGHTS[9] = { 1.0, 0.8, 0.65, 0.5, 0.4, 0.2, 0.1, 0.05, 0.025 }; 

		float4 sampleA = tex2D(_MainTex, input.uv.xy);
		float4 sampleB = tex2D(_MainTex, input.uv01.xy);
		float4 sampleC = tex2D(_MainTex, input.uv01.zw);
		float4 sampleD = tex2D(_MainTex, input.uv23.xy);
		float4 sampleE = tex2D(_MainTex, input.uv23.zw);
		float4 sampleF = tex2D(_MainTex, input.uv45.xy);
		float4 sampleG = tex2D(_MainTex, input.uv45.zw);
		float4 sampleH = tex2D(_MainTex, input.uv67.xy);
		float4 sampleI = tex2D(_MainTex, input.uv67.zw);
		float4 sampleJ = tex2D(_MainTex, input.uv89.xy);
		float4 sampleK = tex2D(_MainTex, input.uv89.zw);

		w = G_WEIGHTS[0]; sum += sampleA.r * w; weights += w;
		w = G_WEIGHTS[1]; sum += sampleB.r * w; weights += w;
		w = G_WEIGHTS[1]; sum += sampleC.r * w; weights += w;
		w = G_WEIGHTS[2]; sum += sampleD.r * w; weights += w;
		w = G_WEIGHTS[2]; sum += sampleE.r * w; weights += w;
		w = G_WEIGHTS[3]; sum += sampleF.r * w; weights += w;
		w = G_WEIGHTS[3]; sum += sampleG.r * w; weights += w;
		w = G_WEIGHTS[4]; sum += sampleH.r * w; weights += w;
		w = G_WEIGHTS[4]; sum += sampleI.r * w; weights += w;
		w = G_WEIGHTS[5]; sum += sampleJ.r * w; weights += w;
		w = G_WEIGHTS[5]; sum += sampleK.r * w; weights += w;

		sum /= weights + 1e-4f;
		return float4(sum, 0, 0, 0);
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	ENDCG
	SubShader {
		ZTest Always Cull Off ZWrite Off Blend Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vertBlur
			#pragma fragment fragBlur
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			// these are all global uniforms
			sampler2D _ObjectTex, _GlowTex, _OrigTex;
			half4 _GlowColor;
			half _GlowIntensity;

			half4 frag (v2f_img input) : SV_Target
			{
				float4 orig = tex2D(_OrigTex, input.uv);
				float obj = tex2D(_ObjectTex, input.uv).r;
				float glow = tex2D(_GlowTex, input.uv).r;
				return orig + _GlowIntensity * _GlowColor * glow * (1.0 - obj);
			}
			ENDCG
		}
	}
	Fallback Off
}