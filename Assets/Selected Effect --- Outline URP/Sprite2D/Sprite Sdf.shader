Shader "Selected Effect --- Outline/Sprite/Sdf Outline" {
	Properties {
		_MainTex ("Main", 2D) = "white" {}
		_Color ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[Header(Outline)]
		[NoScaleOffset] _SDFTex ("SDF", 2D) = "white" {}
		_OutlineColor          ("Color", Color) = (1, 1, 1, 1)
		_OutlineThickness      ("Thickness", Float) = 0.5
		_OutlineSoftness       ("Softness", Float) = 1
		_OutlineOffset         ("Offset", Float) = 0
		_OutlineEdgeSmoothness ("Edge Smoothness", Float) = 0
		_Scale                 ("Scale", Vector) = (1, 1, 1, 1)
	}
	SubShader {
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Sprite"
			"CanUseSpriteAtlas" = "True"
		}
		Pass
		{
			Cull Off Lighting Off ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ OUTLINE_ONLY
			#include "UnityCG.cginc"

			sampler2D _MainTex, _SDFTex;
			half4 _Color, _OutlineColor;
			half _OutlineThickness, _OutlineSoftness, _OutlineOffset, _OutlineEdgeSmoothness;
			half2 _Scale;

			struct v2f
			{
				float4 pos   : SV_POSITION;
				half4 color : COLOR;
				float2 uv    : TEXCOORD0;
			};
			v2f vert (appdata_full v)
			{
				float4 locpos = v.vertex;
				locpos.xy *= _Scale;

				v2f o;
				o.pos = UnityObjectToClipPos(locpos);
				o.uv = v.texcoord;
				o.color = v.color * _Color;
#ifdef PIXELSNAP_ON
				o.pos = UnityPixelSnap(o.pos);
#endif
				return o;
			}
			half4 frag (v2f input) : SV_Target
			{
				half4 c = tex2D(_MainTex, input.uv);
				c.rgb *= c.a;
				c *= input.color;

				half4 oc = 0.0;
				half4 sdf = tex2D(_SDFTex, input.uv);
				oc.rgb = _OutlineColor.rgb;

				half dist = sdf.a;
				half offset = _OutlineOffset*0.5+0.5;
				half size = offset - _OutlineThickness;
				half diffInv = 1.0 / (offset - size);
				half oa = saturate((dist - size) * diffInv);
				oc.a = smoothstep(0.0, _OutlineSoftness, oa);

				half f = smoothstep(offset - _OutlineEdgeSmoothness, offset + _OutlineEdgeSmoothness, dist);
				return lerp(oc, c, f);
			}
			ENDCG
		}
	}
	Fallback Off
}