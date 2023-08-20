Shader "Selected Effect --- Outline/Sprite/Outline" {
	Properties {
		_MainTex ("Main", 2D) = "white" {}
		_Color ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[Header(Outline)]
		_OutlineColor         ("Outline Color", Color) = (1, 1, 1, 1)
		_OutlineThickness     ("Outline Thickness", Float) = 1
		_OutlineDashSpeed     ("Outline Dash Speed", Float) = 30
		_OutlineDashGap       ("Outline Dash Gap", Float) = 1
		_OutlineDashThickness ("Outline Dash Thickness", Float) = 0.5
		_Scale                ("Scale", Vector) = (1, 1, 1, 1)
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
			Blend One OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ OUTLINE_ONLY
			#pragma multi_compile _ OUTLINE_DASH
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			half4 _Color, _OutlineColor, _MainTex_TexelSize;
			half _OutlineThickness, _OutlineDashSpeed, _OutlineDashGap, _OutlineDashThickness;
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
#if OUTLINE_DASH
				float2 v = input.pos.xy + _Time * _OutlineDashSpeed;
				float skip = sin(_OutlineDashGap * abs(distance(0.5, v))) + _OutlineDashThickness;
				half4 oc = _OutlineColor;
				oc.a = lerp(0.0, 1.0, saturate(skip));
#else
				half4 oc = _OutlineColor;
#endif
				oc.a *= ceil(c.a);
				oc.rgb *= oc.a;
 
				half up = tex2D(_MainTex,    input.uv + fixed2(0, _MainTex_TexelSize.y * _OutlineThickness)).a;
				half down = tex2D(_MainTex,  input.uv - fixed2(0, _MainTex_TexelSize.y * _OutlineThickness)).a;
				half right = tex2D(_MainTex, input.uv + fixed2(_MainTex_TexelSize.x * _OutlineThickness, 0)).a;
				half left = tex2D(_MainTex,  input.uv - fixed2(_MainTex_TexelSize.x * _OutlineThickness, 0)).a;
#if OUTLINE_ONLY
				c = 0.0;
#endif
				return lerp(oc, c, ceil(up * down * right * left));
			}
			ENDCG
		}
	}
	Fallback Off
}