using JiongXiaGu.ShaderTools;
using System;
using System.Collections.Generic;

namespace JiongXiaGu.LowpolyOcean
{

    [Serializable]
    [ShaderFieldGroup]
    public sealed class UnderOceanModeOptions
    {
        public const string LightingKeyword = "LPUNDER_OCEAN_LIGHTING";
        public const string FogKeyword = "LPUNDER_OCEAN_EFFECT";
        public const string CookieKeyword = "LPUNDER_OCEAN_COOKIE";
        public const string ClipKeyword = "LPUNDER_OCEAN_CLIP";

        public static IReadOnlyList<string> KeywordAll { get; } = new string[]
        {
            LightingKeyword,
            FogKeyword,
            CookieKeyword,
            ClipKeyword,
        };

        public static UnderOceanModeOptions Default => new UnderOceanModeOptions()
        {
        };

        [ShaderFieldEnumKeyword(null, UnderLightingMode.None,
           LightingKeyword, UnderLightingMode.RuntimeAtten)]
        public UnderLightingMode Lighting;

        [ShaderFieldEnumKeyword(null, UnderCookieMode.None,
           CookieKeyword, UnderCookieMode.Addition)]
        public UnderCookieMode Cookie;

        [ShaderFieldEnumKeyword(null, UnderFogMode.None,
            FogKeyword, UnderFogMode.InPass)]
        public UnderFogMode Fog;

        [ShaderFieldEnumKeyword(null, UnderClipMode.None,
            ClipKeyword, UnderClipMode.Normal)]
        public UnderClipMode Clip;
    }
}
