#ifndef VOLUMETRIC_LIGHTS_COMMONS_URP
#define VOLUMETRIC_LIGHTS_COMMONS_URP

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

CBUFFER_START(UnityPerMaterial)

float4 _ConeTipData, _ConeAxis;
float4 _ExtraGeoData;
float3 _BoundsCenter, _BoundsExtents;
float4 _ToLightDir;

sampler3D _MainTex;
float jitter;
float _NoiseScale, _NoiseStrength, _NoiseFinalMultiplier, _Border, _DistanceFallOff;
half4 _Color;
float4 _AreaExtents;

float _FogStepping;
float3 _WindDirection;
float _DitherStrength, _JitterStrength;
half4 _LightColor;
half  _Density;
int _FlipDepthTexture;

CBUFFER_END

/*
inline float GetLinearEyeDepth(float2 uv) {
    if (_FlipDepthTexture) {
       uv.y = 1.0 - uv.y;
    }
    float depth = SampleSceneDepth(uv);
	float sceneLinearDepth = LinearEyeDepth(depth, _ZBufferParams);
    return sceneLinearDepth;
}
*/

void ClampRayDepth(float4 scrPos, inout float t1) {
    float2 uv =  scrPos.xy / scrPos.w;

/* Legacy code: doesn't work well in VR
    float vz = GetLinearEyeDepth(uv);
    float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
    float2 suv = uv;
    #if UNITY_SINGLE_PASS_STEREO
        // If Single-Pass Stereo mode is active, transform the
        // coordinates to get the correct output UV for the current eye.
        float4 scaleOffset = unity_StereoScaleOffset[unity_StereoEyeIndex];
        suv = (suv - scaleOffset.zw) / scaleOffset.xy;
    #endif
    float3 vpos = float3((suv * 2 - 1) / p11_22, -1) * vz;
    float4 wpos = mul(unity_CameraToWorld, float4(vpos, 1));
*/

    // World position reconstruction
    float depth  = SampleSceneDepth(_FlipDepthTexture ? float2(uv.x, 1.0 - uv.y) : uv);
    float4 raw   = mul(UNITY_MATRIX_I_VP, float4(uv * 2 - 1, depth, 1));
    float3 wpos  = raw.xyz / raw.w;

    float z = distance(_WorldSpaceCameraPos, wpos.xyz);
    t1 = min(t1, z);
} 

#endif // VOLUMETRIC_LIGHTS_COMMONS_URP

