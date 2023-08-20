Shader "Selected Effect --- Outline/Post Process/Flat Color" {
	Properties {}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" }
		Pass {
			Tags { "LightMode" = "FlatColor" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 pos : SV_POSITION;
			};
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			half4 frag (v2f input) : SV_TARGET
			{
				return half4(1, 1, 0, 0);
			}
			ENDCG
		}
	}
	FallBack Off
}