Shader "Selected Effect --- Outline/Aura Outline" {
	Properties{
		[Header(Standard)][Space(5)]
		[MainColor] _BaseColor  ("Color", Color) = (1, 1, 1, 1)
		[MainTexture] _BaseMap  ("Albedo", 2D) = "white" {}
		_Smoothness             ("Smoothness", Range(0.0, 1.0)) = 0.5
		[Gamma] _Metallic       ("Metallic", Range(0.0, 1.0)) = 0.0
		[Toggle(_NORMALMAP)] _1 ("Normal Map", Float) = 0
		_BumpScale              ("Normal Map Scale", Float) = 1.0
		_BumpMap                ("Normal Map", 2D) = "bump" {}
		[Header(Outline)][Space(5)]
		_OutlineWidth ("Outline Width", Range(0.002, 2)) = 0.3
		_OutlineZ     ("Outline Z", Range(-0.06, 0)) = -0.05
		_ColorInside  ("Aura Inside", Color) = (0, 0, 1, 1)
		_ColorOutside ("Aura Outside", Color) = (0, 1, 1, 1)
		_Brightness   ("Brightness", Range(0.5, 3)) = 2
		_Edge         ("Rim Edge", Range(0.0, 1)) = 0.1
		_RimPower     ("Rim Power", Range(0.01, 10.0)) = 1
		[NoScaleOffset]
		_NoiseTex     ("Noise", 2D) = "white" {}
		_Scale        ("Noise Scale", Range(0, 0.05)) = 0.01
		_SpeedX       ("Speed X", Range(-20, 20)) = 0
		_SpeedY       ("Speed Y", Range(-20, 20)) = 3
		_Opacity      ("Opacity", Range(0.01, 10.0)) = 10
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Opaque" }
		Pass {
			Name "StandardLit"
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 3.0

			// -------------------------------------
			// Material Keywords
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _METALLICSPECGLOSSMAP
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature _OCCLUSIONMAP

			#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature _GLOSSYREFLECTIONS_OFF
			#pragma shader_feature _SPECULAR_SETUP
			#pragma shader_feature _RECEIVE_SHADOWS_OFF

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

			// -------------------------------------
			// Unity defined keywords
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex LitPassVertex
			#pragma fragment LitPassFragment

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

			struct Attributes
			{
				float4 positionOS : POSITION;
				float3 normalOS   : NORMAL;
				float4 tangentOS  : TANGENT;
				float2 uv         : TEXCOORD0;
				float2 uvLM       : TEXCOORD1;
				float3 color      : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct Varyings
			{
				float2 uv               : TEXCOORD0;
				float2 uvLM             : TEXCOORD1;
				float4 positionWSAndFog : TEXCOORD2; // xyz: positionWS, w: vertex fog factor
				half3  normalWS         : TEXCOORD3;
#if _NORMALMAP
				half3 tangentWS         : TEXCOORD4;
				half3 bitangentWS       : TEXCOORD5;
#endif
#ifdef _MAIN_LIGHT_SHADOWS
				float4 shadowCoord      : TEXCOORD6; // compute shadow coord per-vertex for the main light
#endif
				float3 barycentric      : TEXCOORD7;
				float4 positionCS       : SV_POSITION;
			};
			Varyings LitPassVertex (Attributes input)
			{
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
				float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

				Varyings output;
				output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
				output.uvLM = input.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				output.positionWSAndFog = float4(vertexInput.positionWS, fogFactor);
				output.normalWS = vertexNormalInput.normalWS;
#ifdef _NORMALMAP
				output.tangentWS = vertexNormalInput.tangentWS;
				output.bitangentWS = vertexNormalInput.bitangentWS;
#endif
#ifdef _MAIN_LIGHT_SHADOWS
				output.shadowCoord = GetShadowCoord(vertexInput);
#endif
				output.positionCS = vertexInput.positionCS;
				output.barycentric = input.color;
				return output;
			}
			half4 LitPassFragment (Varyings input) : SV_Target
			{
				SurfaceData surfaceData;
				InitializeStandardLitSurfaceData(input.uv, surfaceData);

#if _NORMALMAP
				half3 normalWS = TransformTangentToWorld(surfaceData.normalTS, half3x3(input.tangentWS, input.bitangentWS, input.normalWS));
#else
				half3 normalWS = input.normalWS;
#endif
				normalWS = normalize(normalWS);

#ifdef LIGHTMAP_ON
				half3 bakedGI = SampleLightmap(input.uvLM, normalWS);
#else
				half3 bakedGI = SampleSH(normalWS);
#endif

				float3 positionWS = input.positionWSAndFog.xyz;
				half3 viewDirectionWS = SafeNormalize(GetCameraPositionWS() - positionWS);

				BRDFData brdfData;
				InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

#ifdef _MAIN_LIGHT_SHADOWS
				Light mainLight = GetMainLight(input.shadowCoord);
#else
				Light mainLight = GetMainLight();
#endif

				half3 color = GlobalIllumination(brdfData, bakedGI, surfaceData.occlusion, normalWS, viewDirectionWS);
				color += LightingPhysicallyBased(brdfData, mainLight, normalWS, viewDirectionWS);

#ifdef _ADDITIONAL_LIGHTS
				int additionalLightsCount = GetAdditionalLightsCount();
				for (int i = 0; i < additionalLightsCount; ++i)
				{
					Light light = GetAdditionalLight(i, positionWS);
					color += LightingPhysicallyBased(brdfData, light, normalWS, viewDirectionWS);
				}
#endif
				color += surfaceData.emission;
				float fogFactor = input.positionWSAndFog.w;
				color = MixFog(color, fogFactor);
				return half4(color, surfaceData.alpha);
			}
			ENDHLSL
		}
		Pass {
			Tags { "LightMode" = "LightweightForward" }
			Cull Back ZWrite Off Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_FOG_COORDS(0)
				float3 view : TEXCOORD1;
				float3 norm : TEXCOORD2;
			};

			float _OutlineWidth, _OutlineZ, _RimPower;
			sampler2D _NoiseTex;
			float _Scale, _Opacity, _Edge;
			float4 _ColorInside, _ColorOutside;
			float _Brightness, _SpeedX, _SpeedY;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 nrm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				float2 offset = TransformViewToProjection(nrm.xy);
				o.pos.xy += offset * _OutlineWidth * o.pos.z;
				o.pos.z += _OutlineZ;
				o.norm = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
				o.view = normalize(WorldSpaceViewDir(v.vertex));
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			half4 frag (v2f input) : SV_Target
			{
				float2 uv = float2(input.pos.x * _Scale - (_Time.x * _SpeedX), input.pos.y * _Scale - (_Time.x * _SpeedY));
				float4 nis = tex2D(_NoiseTex, uv);
				float4 rim = pow(saturate(dot(input.view, input.norm)), _RimPower);
				rim -= nis;
				float4 rim1 = saturate(rim.a * _Opacity);
				float4 rim2 = (saturate((_Edge + rim.a) * _Opacity) - rim1) * _Brightness;
				float4 c = (_ColorInside * rim1) + (_ColorOutside * rim2);
				UNITY_APPLY_FOG(input.fogCoord, c);
				return c;
			}
			ENDCG
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	}
	Fallback Off
}