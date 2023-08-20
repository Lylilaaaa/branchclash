Shader "Selected Effect --- Outline/Extrude Vertex/Outline Only" {
	Properties {
		_OutlineWidth ("Outline Width", Float) = 0.1
		_OutlineColor ("Outline Color", Color) = (0, 1, 0, 0)
		_OutlineFactor ("Outline Factor", Range(0, 1)) = 1
		[MaterialToggle] _OutlineWriteZ ("Outline Z Write", Float) = 1.0
		_DepthOffset ("Depth Offset", Float) = -8
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" }
		Pass {
			Tags { "LightMode" = "SRPDefaultUnlit" }   // SRPDefaultUnlit pass is called before UniversalForward pass
			Cull Back Blend Zero One Offset [_DepthOffset], [_DepthOffset]
		}
		Pass {
			Tags { "LightMode" = "UniversalForward" }
			Cull Front Blend One OneMinusDstColor ZWrite [_OutlineWriteZ]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ OUTLINE_DASH
			#include "Outline.cginc"
			ENDCG
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	} 
	FallBack Off
}
