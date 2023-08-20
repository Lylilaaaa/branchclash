#ifndef OUTLINE_INCLUDED
#define OUTLINE_INCLUDED

#include "UnityCG.cginc"

float3 _OutlineColor;
float _OutlineWidth, _OutlineFactor, _OutlineBasedVertexColorR;
half4 _OutlineDashParams;

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

	// view space vertex position
	float3 vp = UnityObjectToViewPos(v.vertex);
	o.pos = UnityViewToClipPos(vp + dir * -vp.z * ow * 0.001);
	return o;
}
float4 frag (v2f input) : SV_TARGET
{
#if OUTLINE_DASH
	float2 pos = input.pos.xy + _Time.y * _OutlineDashParams.xy;
	float skip = sin(_OutlineDashParams.z * abs(distance(0, pos))) + _OutlineDashParams.w;
	clip(skip);
#endif
	return float4(_OutlineColor, 1.0);
}

#endif