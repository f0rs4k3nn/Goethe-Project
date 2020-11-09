using UnityEngine;

namespace VolumetricLights {

    public delegate void OnSettingsChanged();

    public enum ShadowResolution {
        _32 = 32,
        _64 = 64,
        _128 = 128,
        _256 = 256,
        _512 = 512,
        _1024 = 1024,
        _2048 = 2048
    }

    public enum ShadowBakeInterval {
        OnStart,
        EveryFrame
    }

    [CreateAssetMenu(menuName = "Volumetric Light Profile", fileName = "VolumetricLightProfile", order = 335)]
    public class VolumetricLightProfile : ScriptableObject {

        public enum BlendMode {
            Additive = 0,
            Blend = 1
        }

        [Header("Rendering")]
        public BlendMode blendMode = BlendMode.Additive;
        [Range(1, 256)] public int raymarchQuality = 8;
        [Range(0, 2)] public float dithering = 1f;
        [Range(0, 2)] public float jittering = 0.5f;

        [Tooltip("Uses blue noise for jittering computation reducing moiré pattern of normal jitter. Usually not needed unless you use shadow occlusion. It adds an additional texture fetch so use only if it provides you a clear visual improvement.")]
        public bool useBlueNoise;

        [Tooltip("The render queue for this renderer. By default, all transparent objects use a render queue of 3000. Use a lower value to render before all transparent objects.")]
        public int renderQueue = 3101;

        [Tooltip("Optional sorting layer Id (number) for this renderer. By default 0. Usually used to control the order with other transparent renderers, like Sprite Renderer.")]
        public int sortingLayerID;

        [Tooltip("Optional sorting order for this renderer. Used to control the order with other transparent renderers, like Sprite Renderer.")]
        public int sortingOrder;

        [Tooltip("Use only if depth texture is inverted. Alternatively you can enable MSAA, HDR or change the render scale in the pipeline asset.")]
        public bool flipDepthTexture;

        [Tooltip("Ignores light enable state. Always show volumetric fog. This option is useful to produce fake volumetric lights.")]
        public bool alwaysOn;

        [Header("Appearance")]
        public bool useNoise = true;
        public Texture noiseTexture;
        [Range(0, 3)] public float noiseStrength = 1f;
        public float noiseScale = 5f;
        public float noiseFinalMultiplier = 1f;

        public float density = 0.2f;

        public Color mediumAlbedo = Color.white;

        [Tooltip("Overall brightness multiplier.")]
        public float brightness = 1f;

        [Tooltip("Brightiness increase when looking against light direction.")]
        public float diffusionIntensity;

        [Range(0, 1f)] public float distanceFallOff = 1f;

        [Range(0, 1f), Tooltip("Smooth edges")] public float border = 0.5f;

        [Header("Spot Light")]
        [Tooltip("Radius of the tip of the cone. Only applies to spot lights.")] public float tipRadius;

        [Header("Area Light")]
        [Range(0f, 80f)] public float frustumAngle;

        [Header("Animation")]
        public Vector3 windDirection = new Vector3(0.03f, 0.02f, 0);

        [Header("Dust Particles")]
        public bool enableDustParticles;
        public float dustBrightness = 1f;
        public float dustMinSize = 0.01f;
        public float dustMaxSize = 0.02f;
        public float dustWindSpeed = 1f;

        [Header("Shadow Occlusion")]
        public bool enableShadows;
        [Range(0, 1)] public float shadowIntensity = 0.7f;
        public ShadowResolution shadowResolution = ShadowResolution._256;
        public LayerMask shadowCullingMask = ~2;
        public ShadowBakeInterval shadowBakeInterval = ShadowBakeInterval.OnStart;
        public float shadowNearDistance = 0.1f;


        private void OnEnable() {
            if (noiseTexture == null) {
                noiseTexture = Resources.Load<Texture3D>("Textures/NoiseTex3D1");
            }
        }

        private void OnValidate() {
            tipRadius = Mathf.Max(0, tipRadius);
            density = Mathf.Max(0, density);
            noiseScale = Mathf.Max(0.1f, noiseScale);
            diffusionIntensity = Mathf.Max(0, diffusionIntensity);
            dustMaxSize = Mathf.Max(dustMaxSize, dustMinSize);

            // Update all lights using this profile
            VolumetricLight[] vls = FindObjectsOfType<VolumetricLight>();
                for (int k = 0; k < vls.Length; k++) {
                if (vls[k] != null && vls[k].profile == this) {
                    vls[k].UpdateMaterialProperties();
                }
            }


        }

    }
}