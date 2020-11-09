//------------------------------------------------------------------------------------------------------------------
// Volumetric Lights
// Created by Kronnect
//------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Collections.Generic;

namespace VolumetricLights {

    [ExecuteInEditMode, RequireComponent(typeof(Light)), AddComponentMenu("Effects/Volumetric Light", 1000)]
    public partial class VolumetricLight : MonoBehaviour {

        // Common
        public bool useCustomBounds;
        public Bounds bounds;
        public VolumetricLightProfile profile;
        public float customRange = 1f;
        [Tooltip("Currently only used for point light occlusion orientation.")]
        public Transform targetCamera;

        // Area
        public bool useCustomSize;
        public float areaWidth = 1f, areaHeight = 1f;

        [NonSerialized]
        public Light lightComp;

        const string SKW_NOISE = "VL_NOISE";
        const string SKW_BLUENOISE = "VL_BLUENOISE";
        const string SKW_SPOT = "VL_SPOT";
        const string SKW_POINT = "VL_POINT";
        const string SKW_AREA_RECT = "VL_AREA_RECT";
        const string SKW_AREA_DISC = "VL_AREA_DISC";
        const string SKW_SHADOWS = "VL_SHADOWS";
        const string SKW_DIFFUSION = "VL_DIFFUSION";

        MeshFilter mf;
        MeshRenderer meshRenderer;
        Material fogMat, fogMatLight, fogMatEmpty;
        Vector3 windDirectionAcum;
        bool requireUpdateMaterial;
        List<string> keywords;
        static Texture2D blueNoiseTex;

        void OnEnable() {
            lightComp = GetComponent<Light>();
            if (gameObject.layer == 0) { // if object is in default layer, move it to transparent fx layer
                gameObject.layer = 1;
            }
            Refresh();
        }

        public void Refresh() {
            CheckProfile();
            DestroyMesh();
            CheckMesh();
            UpdateMaterialPropertiesNow();
        }


        private void OnValidate() {
            requireUpdateMaterial = true;
        }

        private void OnDisable() {
            TurnOff();
        }

        void TurnOff() {
            if (meshRenderer != null) {
                meshRenderer.enabled = false;
            }
            ShadowsDisable();
            ParticlesDisable();
        }

        private void OnDestroy() {
            if (fogMatEmpty != null) {
                DestroyImmediate(fogMatEmpty);
                fogMatEmpty = null;
            }
            if (fogMatLight != null) {
                DestroyImmediate(fogMatLight);
                fogMatLight = null;
            }
            if (meshRenderer != null) {
                meshRenderer.enabled = false;
            }
            ShadowsDispose();
            ParticlesDispose();
        }

        void LateUpdate() {

            bool isActiveAndEnabled = lightComp.isActiveAndEnabled || (profile != null && profile.alwaysOn);
            if (isActiveAndEnabled) {
                if (meshRenderer != null && !meshRenderer.enabled) {
                    requireUpdateMaterial = true;
                }
            } else {
                if (meshRenderer != null && meshRenderer.enabled) {
                    TurnOff();
                }
                return;
            }

            if (CheckMesh()) {
                if (!Application.isPlaying) {
                    ParticlesDispose();
                }
                ScheduleShadowCapture();
                requireUpdateMaterial = true;
            }

            if (requireUpdateMaterial) {
                requireUpdateMaterial = false;
                UpdateMaterialPropertiesNow();
            }

            if (fogMat == null || meshRenderer == null || profile == null) return;

            UpdateVolumeGeometry();
            UpdateDiffusionTerm();

            fogMat.SetColor("_LightColor", lightComp.color * profile.mediumAlbedo * (lightComp.intensity * profile.brightness));
            windDirectionAcum += profile.windDirection * Time.deltaTime;
            fogMat.SetVector("_WindDirection", windDirectionAcum);

            if (profile.enableShadows) {
                ShadowsUpdate();
            }
        }


        void UpdateDiffusionTerm() {
            Vector4 toLightDir = -transform.forward;
            toLightDir.w = profile.diffusionIntensity;
            fogMat.SetVector("_ToLightDir", toLightDir);
        }


        public void UpdateVolumeGeometry() {
            UpdateVolumeGeometryMaterial(fogMat);
            if (profile.enableDustParticles && particleMaterial != null) {
                UpdateVolumeGeometryMaterial(particleMaterial);
                particleMaterial.SetMatrix("_WorldToLocal", transform.worldToLocalMatrix);
            }
        }

        void UpdateVolumeGeometryMaterial(Material mat) {
            if (mat == null) return;

            Vector4 tipData = transform.position;
            tipData.w = profile.tipRadius;
            mat.SetVector("_ConeTipData", tipData);

            if (customRange < 0.001f) customRange = 0.001f;
            Vector4 coneAxis = transform.forward * customRange;
            coneAxis.w = customRange * customRange;
            mat.SetVector("_ConeAxis", coneAxis);

            float maxDistSqr = generatedRange * generatedRange;
            float falloff = Mathf.Max(0.0001f, profile.distanceFallOff);
            float pointAttenX = -1f / (maxDistSqr * falloff);
            float pointAttenY = maxDistSqr / (maxDistSqr * falloff);
            mat.SetVector("_ExtraGeoData", new Vector4(generatedBaseRadius, pointAttenX, pointAttenY));

            if (!useCustomBounds) {
                bounds = meshRenderer.bounds;
            }
            mat.SetVector("_BoundsCenter", bounds.center);
            mat.SetVector("_BoundsExtents", bounds.extents);
            if (generatedType == LightType.Area) {
                float baseMultiplierComputed = (generatedAreaFrustumMultiplier - 1f) / generatedRange;
                mat.SetVector("_AreaExtents", new Vector4(areaWidth * 0.5f, areaHeight * 0.5f, generatedRange, baseMultiplierComputed));
            } else if (generatedType == LightType.Disc) {
                float baseMultiplierComputed = (generatedAreaFrustumMultiplier - 1f) / generatedRange;
                mat.SetVector("_AreaExtents", new Vector4(areaWidth * areaWidth, areaHeight, generatedRange, baseMultiplierComputed));
            }
        }


        public void UpdateMaterialProperties() {
            requireUpdateMaterial = true;
        }

        void UpdateMaterialPropertiesNow() {

            bool alwaysOn = profile != null && profile.alwaysOn;
            if (this == null || !isActiveAndEnabled || lightComp == null || (!lightComp.isActiveAndEnabled && !alwaysOn)) return;

            if (meshRenderer == null) {
                meshRenderer = GetComponent<MeshRenderer>();
            }

            if (profile == null) {
                if (meshRenderer != null) {
                    if (fogMatEmpty == null) {
                        fogMatEmpty = new Material(Shader.Find("VolumetricLights/Empty"));
                        fogMatEmpty.hideFlags = HideFlags.DontSave;
                    }
                    meshRenderer.sharedMaterial = fogMatEmpty;
                }
                return;
            }

            if (fogMatLight == null) {
                fogMatLight = new Material(Shader.Find("VolumetricLights/VolumetricLightURP"));
                fogMatLight.hideFlags = HideFlags.DontSave;
            }
            fogMat = fogMatLight;

            if (meshRenderer != null) {
                meshRenderer.sharedMaterial = fogMat;
            }

            if (fogMat == null || profile == null) return;

            if (meshRenderer != null) {
                meshRenderer.sortingLayerID = profile.sortingLayerID;
                meshRenderer.sortingOrder = profile.sortingOrder;
            }
            fogMat.renderQueue = profile.renderQueue;

            fogMat.SetInt("_BlendDest", profile.blendMode == VolumetricLightProfile.BlendMode.Additive ? (int)UnityEngine.Rendering.BlendMode.One : (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            fogMat.SetTexture("_MainTex", profile.noiseTexture);
            fogMat.SetFloat("_NoiseStrength", profile.noiseStrength);
            fogMat.SetFloat("_NoiseScale", 0.1f / profile.noiseScale);
            fogMat.SetFloat("_NoiseFinalMultiplier", profile.noiseFinalMultiplier);
            fogMat.SetFloat("_Border", Mathf.Max(0.0001f, profile.border));
            fogMat.SetFloat("_DistanceFallOff", Mathf.Max(0.0001f, profile.distanceFallOff));
            fogMat.SetFloat("_Density", profile.density);
            fogMat.SetFloat("_FogStepping", profile.raymarchQuality);
            fogMat.SetFloat("_DitherStrength", profile.dithering * 0.01f);
            fogMat.SetFloat("_JitterStrength", profile.jittering);
            if (profile.jittering > 0) {
                if (blueNoiseTex == null) blueNoiseTex = Resources.Load<Texture2D>("Textures/blueNoiseVL");
                fogMat.SetTexture("_BlueNoise", blueNoiseTex);
            }
            fogMat.SetInt("_FlipDepthTexture", profile.flipDepthTexture ? 1 : 0);

            if (keywords == null) {
                keywords = new List<string>();
            } else {
                keywords.Clear();
            }
            if (profile.useBlueNoise) keywords.Add(SKW_BLUENOISE);
            if (profile.useNoise) keywords.Add(SKW_NOISE);
            switch (lightComp.type) {
                case LightType.Spot: keywords.Add(SKW_SPOT); break;
                case LightType.Point: keywords.Add(SKW_POINT); break;
                case LightType.Area: keywords.Add(SKW_AREA_RECT); break;
                case LightType.Disc: keywords.Add(SKW_AREA_DISC); break;
            }
            if (profile.diffusionIntensity > 0) {
                keywords.Add(SKW_DIFFUSION);
            }
            if (useCustomBounds) {
                keywords.Add(SKW_CUSTOM_BOUNDS);
            }

            ShadowsSupportCheck();
            if (profile.enableShadows) {
                keywords.Add(SKW_SHADOWS);
            }
            fogMat.shaderKeywords = keywords.ToArray();

            ParticlesCheckSupport();
        }


        void CheckProfile() {
            if (profile == null) {
                profile = ScriptableObject.CreateInstance<VolumetricLightProfile>();
            }
        }
    }
}
