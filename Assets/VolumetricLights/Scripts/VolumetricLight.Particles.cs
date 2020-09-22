//------------------------------------------------------------------------------------------------------------------
// Volumetric Lights
// Created by Kronnect
//------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using System;

namespace VolumetricLights {

    public partial class VolumetricLight : MonoBehaviour {

        #region Particle support

        const string PARTICLE_SYSTEM_NAME = "DustParticles";
        const string SKW_CUSTOM_BOUNDS = "VL_CUSTOM_BOUNDS";

        Material particleMaterial;

        [NonSerialized]
        public ParticleSystem ps;

        void ParticlesDisable() {
            if (ps != null) {
                ps.Stop();
            }
            if (particleMaterial != null) {
                particleMaterial.SetColor("_ParticleLightColor", new Color(0, 0, 0, 0));
            }
        }

        void ParticlesDispose() {
            if (ps != null) {
                DestroyImmediate(ps.gameObject);
            }
        }

        void ParticlesCheckSupport() {
            if (!profile.enableDustParticles) {
                ParticlesDispose();
                return;
            }

            bool psNew = false;
            if (ps == null) {
                Transform childPS = transform.Find(PARTICLE_SYSTEM_NAME);
                if (childPS != null) {
                    ps = childPS.GetComponent<ParticleSystem>();
                    if (ps == null) {
                        DestroyImmediate(childPS.gameObject);
                    }
                }
                if (ps == null) {
                    GameObject psObj = Resources.Load<GameObject>("Prefabs/DustParticles") as GameObject;
                    if (psObj == null) return;
                    psObj = Instantiate<GameObject>(psObj);
                    psObj.name = PARTICLE_SYSTEM_NAME;
                    psObj.hideFlags = HideFlags.DontSave;
                    psObj.transform.SetParent(transform, false);
                    ps = psObj.GetComponent<ParticleSystem>();
                }
                psNew = true;
            }

            if (particleMaterial == null) {
                particleMaterial = Instantiate(Resources.Load<Material>("Materials/DustParticle")) as Material;
            }

            // Configure particle material
            if (useCustomBounds) {
                keywords.Add(SKW_CUSTOM_BOUNDS);
            }

            switch (generatedType) {
                case LightType.Spot:
                    keywords.Add(SKW_SPOT);
                    break;
                case LightType.Point:
                    keywords.Add(SKW_POINT);
                    break;
                case LightType.Area:
                    keywords.Add(SKW_AREA_RECT);
                    break;
                case LightType.Disc:
                    keywords.Add(SKW_AREA_DISC);
                    break;
            }
            if (profile.enableShadows) {
                keywords.Add(SKW_SHADOWS);
            }
            particleMaterial.shaderKeywords = keywords.ToArray();

            particleMaterial.renderQueue = profile.renderQueue + 1;
            particleMaterial.SetFloat("_Border", Mathf.Max(0.0001f, profile.border));
            particleMaterial.SetFloat("_DistanceFallOff", Mathf.Max(0.0001f, profile.distanceFallOff));
            particleMaterial.SetColor("_ParticleLightColor", lightComp.color * profile.mediumAlbedo * (lightComp.intensity * profile.dustBrightness));
            ps.GetComponent<ParticleSystemRenderer>().material = particleMaterial;

            // Main properties
            ParticleSystem.MainModule main = ps.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            ParticleSystem.MinMaxCurve startSize = main.startSize;
            startSize.mode = ParticleSystemCurveMode.TwoConstants;
            startSize.constantMin = profile.dustMinSize;
            startSize.constantMax = profile.dustMaxSize;
            main.startSize = startSize;

            // Set emission bounds
            ParticleSystem.ShapeModule shape = ps.shape;
            switch (generatedType) {
                case LightType.Spot:
                    shape.shapeType = ParticleSystemShapeType.ConeVolume;
                    shape.angle = generatedSpotAngle * 0.5f;
                    shape.position = Vector3.zero;
                    shape.radius = profile.tipRadius;
                    shape.length = generatedRange;
                    shape.scale = Vector3.one;
                    break;
                case LightType.Point:
                    shape.shapeType = ParticleSystemShapeType.Sphere;
                    shape.position = Vector3.zero;
                    shape.scale = Vector3.one;
                    shape.radius = generatedRange;
                    break;
                case LightType.Area:
                case LightType.Disc:
                    shape.shapeType = ParticleSystemShapeType.Box;
                    shape.position = new Vector3(0, 0, generatedRange * 0.5f);
                    shape.scale = GetComponent<MeshFilter>().sharedMesh.bounds.size;
                    break;
            }

            // Set wind speed
            ParticleSystem.VelocityOverLifetimeModule velocity = ps.velocityOverLifetime;
            Vector3 windDirection = transform.InverseTransformDirection(profile.windDirection);
            shape.position -= windDirection * profile.dustWindSpeed * 10f;
            ParticleSystem.MinMaxCurve velx = velocity.x;
            velx.constantMin = -0.1f + windDirection.x * profile.dustWindSpeed;
            velx.constantMax = 0.1f + windDirection.x * profile.dustWindSpeed;
            velocity.x = velx;
            ParticleSystem.MinMaxCurve vely = velocity.y;
            vely.constantMin = -0.1f + windDirection.y * profile.dustWindSpeed;
            vely.constantMax = 0.1f + windDirection.y * profile.dustWindSpeed;
            velocity.y = vely;
            ParticleSystem.MinMaxCurve velz = velocity.z;
            velz.constantMin = -0.1f + windDirection.z * profile.dustWindSpeed;
            velz.constantMax = 0.1f + windDirection.z * profile.dustWindSpeed;
            velocity.z = velz;

            if (psNew || !Application.isPlaying) {
                ps.Clear();
                ps.Simulate(100);
            }
            ps.Play();
        }
        #endregion

    }


}
