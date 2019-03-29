using JiongXiaGu.ShaderTools;
using System;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    [Serializable]
    [ShaderFieldGroup]
    public sealed class UnderOceanShaderOptions
    {
        public static ShaderAccessor Accessor { get; } = new ShaderAccessor(typeof(UnderOceanShaderOptions));
        public static ShaderAccessor ModeAccessor { get; } = Accessor.CreateAccessor(nameof(Mode));

        public static readonly int UnderOceanPositionShaderID = Shader.PropertyToID("_LPUnderOceanPosition");

        public static int SenderHashCode { get; set; }
        public static int ShaderFieldVersion { get; set; } = -1;
        public int Version { get; set; }
        public UnderOceanModeOptions Mode = UnderOceanModeOptions.Default;
        public UnderLightingOptions Lighting = UnderLightingOptions.Default;
        public UnderFogOptions Fog = UnderFogOptions.Default;
        public UnderCookieOptions Cookie = UnderCookieOptions.Default;

        public void Dirty()
        {
            Version++;
        }

        public static void SetShaderFieldsDirty()
        {
            ShaderFieldVersion = -1;
        }

        public static bool UpdateShaderFields(UnderOceanShaderOptions options)
        {
            var hashCode = options.GetHashCode();
            if (hashCode != SenderHashCode || ShaderFieldVersion != options.Version)
            {
                Accessor.SetGlobalValues(options);
                SenderHashCode = hashCode;
                ShaderFieldVersion = options.Version;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DisableUnderOceanEffectAll()
        {
            foreach (var keyword in UnderOceanModeOptions.KeywordAll)
            {
                Shader.DisableKeyword(keyword);
            }
            SetShaderFieldsDirty();
        }

        public static void DisableUnderOceanFogEffect()
        {
            Shader.DisableKeyword(UnderOceanModeOptions.FogKeyword);
            SetShaderFieldsDirty();
        }

        public class DisableUnderOceanEffectScope : IDisposable
        {
            private bool isDisposed;
            private int original;

            public DisableUnderOceanEffectScope()
            {
                original = 0;
                for (int i = 0; i < UnderOceanModeOptions.KeywordAll.Count; i++)
                {
                    string keyword = UnderOceanModeOptions.KeywordAll[i];
                    if (Shader.IsKeywordEnabled(keyword))
                    {
                        original |= 1 << i;
                    }
                    Shader.DisableKeyword(keyword);
                }
            }

            public void Dispose()
            {
                if (!isDisposed)
                {
                    for (int i = 0; i < UnderOceanModeOptions.KeywordAll.Count; i++)
                    {
                        if ((original & (1 << i)) != 0)
                        {
                            string keyword = UnderOceanModeOptions.KeywordAll[i];
                            Shader.EnableKeyword(keyword);
                        }
                    }
                    isDisposed = true;
                }
            }
        }

        public class DisableUnderOceanFogScope : IDisposable
        {
            public string FogKeyword => UnderOceanModeOptions.FogKeyword;
            public bool OriginalFog { get; private set; }

            public DisableUnderOceanFogScope()
            {
                OriginalFog = Shader.IsKeywordEnabled(FogKeyword);
                if (OriginalFog)
                {
                    Shader.DisableKeyword(FogKeyword);
                }
            }

            public DisableUnderOceanFogScope(bool originalFog)
            {
                OriginalFog = originalFog;
                Shader.DisableKeyword(FogKeyword);
            }

            public void Dispose()
            {
                if (OriginalFog)
                    Shader.EnableKeyword(FogKeyword);
            }
        }
    }
}
