Shader "Selected Effect --- Outline/Extrude Vertex/Outline Transparent" {
	Properties {
		[Header(Standard)][Space(5)]
		[MainColor] _BaseColor  ("Color", Color) = (1, 1, 1, 1)
		[MainTexture] _BaseMap  ("Albedo", 2D) = "white" {}
		_Smoothness             ("Smoothness", Range(0.0, 1.0)) = 0.5
		[Gamma] _Metallic       ("Metallic", Range(0.0, 1.0)) = 0.0
		[Toggle(_NORMALMAP)] _1 ("Normal Map", Float) = 0
		_BumpScale              ("Normal Map Scale", Float) = 1.0
		_BumpMap                ("Normal Map", 2D) = "bump" {}
		[Header(Outline)][Space(5)]
		_OutlineWidth  ("Outline Width", Float) = 0.1
		_OutlineColor  ("Outline Color", Color) = (0, 1, 0, 1)
		_OutlineFactor ("Outline Factor", Range(0, 1)) = 1
		_Overlay       ("Overlay", Float) = 0
		_OverlayColor  ("Overlay Color", Color) = (1, 1, 0, 1)
		_Transparent   ("Transparent", Range(0, 1)) = 0.5
		[MaterialToggle] _OutlineBasedVertexColorR ("Outline Based Vertex Color R", Float) = 0.0
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Transparent" "Queue" = "Transparent+1" }

		// 1st pass --- depth only pass
		Pass {
			Tags { "LightMode" = "SRPDefaultUnlit" }
			ZWrite On ColorMask Off
		}

		// 2nd pass --- draw transparent stuff
		Pass {
			Name "StandardLit"
			Tags { "LightMode" = "UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha Cull Back ZWrite Off ColorMask RGBA ZTest Equal

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

			float4 _OverlayColor;
			float _Overlay, _Transparent;

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
				color = MixFog(color, input.positionWSAndFog.w);
				return half4(color, surfaceData.alpha * _Transparent);
			}
			ENDHLSL
		}

		// 3rd pass --- draw outline
		Pass {
			Tags { "LightMode" = "LightweightForward" }
			ZWrite On ColorMask RGB ZTest Less Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Outline.cginc"
			ENDCG
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	} 
	FallBack Off
}
