using JiongXiaGu.ShaderTools;
using System;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    [Serializable]
    [ShaderFieldGroup]
    public class LightingOptions
    {
        public const string AlbedoShaderFieldName = "_OceanAlbedo";
        public const string SpecularShaderFieldName = "_OceanSpecular";
        public const string SmoothnessShaderFieldName = "_OceanSmoothness";
        public const string OcclusionShaderFieldName = "_OceanOcclusion";
        public const string EmissionShaderFieldName = "_OceanEmission";
        public const string FresnelTextureShaderFieldName = "_OceanFresnelTexture";
        public const string FresnelShaderFieldName = "_OceanFresnelIntensity";
        public const string FresnelColorShaderFieldName = "_OceanFresnelColor";
        public const string ShadowAttenuationShaderFieldName = "_OceanShadowAttenuation";

        public static LightingOptions Default => new LightingOptions()
        {
            Albedo = new Color(0.52f, 0.73f, 0.8f, 1),
            Specular = new Color(1, 1, 1, 1),
            Smoothness = 0.4f,
            Occlusion = 1,
            ShadowAttenuation = 0.62f,
            FresnelTexture = default(Texture),
            Fresnel = 0.7f,
            FresnelColor = new Color(0.32f, 0.51f, 0.61f, 1),
        };

        [ShaderField(AlbedoShaderFieldName)] public Color Albedo;
        [ShaderField(SpecularShaderFieldName)] public Color Specular;
        [ShaderField(SmoothnessShaderFieldName)] [Range(0, 1)] public float Smoothness;
        [ShaderField(OcclusionShaderFieldName)] [Range(0, 1)] public float Occlusion;
        [ShaderField(ShadowAttenuationShaderFieldName)] [Range(0, 1)] public float ShadowAttenuation;
        [ShaderField(FresnelTextureShaderFieldName, LightingMode.UnityPBSAndFresnel)] public Texture FresnelTexture;
        [ShaderField(FresnelShaderFieldName, LightingMode.UnityPBSAndFresnel)] [Range(0, 5)] public float Fresnel;
        [ShaderField(FresnelColorShaderFieldName, LightingMode.UnityPBSAndFresnel)] public Color FresnelColor;
    }
}
