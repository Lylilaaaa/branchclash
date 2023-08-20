Shader "Selected Effect --- Outline/Occlusion/Outline" {
	Properties {
		[Header(Standard)][Space(5)]
		[MainColor] _BaseColor  ("Color", Color) = (1, 1, 1, 1)
		[MainTexture] _BaseMap  ("Albedo", 2D) = "white" {}
		_Smoothness             ("Smoothness", Range(0.0, 1.0)) = 0.5
		[Gamma] _Metallic       ("Metallic", Range(0.0, 1.0)) = 0.0
		[Toggle(_NORMALMAP)] _1 ("Normal Map", Float) = 1
		_BumpScale              ("Scale", Float) = 1.0
		_BumpMap                ("Normal Map", 2D) = "bump" {}
		[Header(Outline)][Space(5)]
		_OutlineColor  ("Outline Color", Color) = (1, 1, 1, 1)
		_OutlineWidth  ("Outline Width", Range(0, 10)) = 2
		_OutlineFactor ("Outline Factor", Range(0, 1)) = 1
		_Overlay       ("Overlay", Range(0, 1)) = 0
		_OverlayColor  ("Overlay Color", Color) = (0, 1, 0, 1)
		[MaterialToggle] _OutlineBasedVertexColorR ("Outline Based Vertex Color R", Float) = 0.0
		[Header(Mask)][Space(5)]
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest2ndPass ("ZTest 2nd Pass", Int) = 0
		[Header(Fill)][Space(5)]
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest3rdPass ("ZTest 3rd Pass", Int) = 0
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" }

		// 1st pass
		Pass {
			Tags { "LightMode" = "SRPDefaultUnlit" }

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
			float _Overlay;

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
				surfaceData.albedo = lerp(surfaceData.albedo.rgb, _OverlayColor.rgb, _Overlay);
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
				return half4(color, surfaceData.alpha);
			}
			ENDHLSL
		}

		// 2nd pass
		Pass {
			Tags { "LightMode" = "UniversalForward" }
			Cull Off ZTest [_ZTest2ndPass] ZWrite Off ColorMask 0
			Stencil {
				Ref 1
				Pass Replace
			}
		}
		
		// 3rd pass
		Pass {
			Tags { "LightMode" = "LightweightForward" }
			Cull Off ZTest [_ZTest3rdPass] ZWrite Off Blend SrcAlpha OneMinusSrcAlpha ColorMask RGB
			Stencil {
				Ref 1
				Comp NotEqual
			}
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			fixed4 _OutlineColor;
			float _OutlineWidth, _OutlineFactor, _OutlineBasedVertexColorR;

			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert (appdata_full v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				// extrude direction
				float3 dir1 = normalize(v.vertex.xyz);
				float3 dir2 = v.normal;
				float3 dir = lerp(dir1, dir2, _OutlineFactor);
				dir = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, dir));

				// is outline based on R channel of vertex color ?
				float ow = _OutlineWidth * v.color.r * _OutlineBasedVertexColorR + (1.0 - _OutlineBasedVertexColorR);

				float3 vp = UnityObjectToViewPos(v.vertex);

				o.pos = UnityViewToClipPos(vp + dir * -vp.z * ow * 0.001);
				return o;
			}
			half4 frag (v2f input) : SV_Target
			{
				return _OutlineColor;
			}
			ENDCG
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	}
	FallBack Off
}
