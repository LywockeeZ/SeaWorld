using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using JiongXiaGu.ShaderTools;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// ocean info settings, need to ensure that the <see cref="SendValuesChanged"/> is called after the parameter has been modified;
    /// </summary>
    [CreateAssetMenu(menuName = EditorModeHelper.AssetMenuNameRoot + nameof(OceanGeneralSettings))]
    public class OceanGeneralSettings : OceanSettings
    {
        protected OceanGeneralSettings()
        {
        }

        [SerializeField] protected OceanShaderOptions shaderOptions;
        public int Version { get; protected set; } = 1;

        public OceanShaderOptions ShaderOptions
        {
            get { return shaderOptions; }
            set { shaderOptions = value; }
        }

        [ContextMenu(nameof(SendValuesChanged))]
        public virtual void SendValuesChanged()
        {
            Version++;
        }

        protected virtual void OnValidate()
        {
            SendValuesChanged();
        }

        public override float GetVertexHeight(Vector3 worldPos)
        {
            return shaderOptions.GetVertexHeight(worldPos);
        }

        private static readonly Lazy<List<string>> keyworldTemp = new Lazy<List<string>>();

        public override void UpdateMateria(Material material, MaterialUpdateOptions options)
        {
            if (options.UpdateKeyword)
            {
                OceanShaderOptions.Accessor.Copy(shaderOptions, material, keyworldTemp.Value, group => ShaderAccessor.FilterByMask(group, (int)options.UpdateContents));
                keyworldTemp.Value.Clear();
            }
            else
            {
                OceanShaderOptions.Accessor.CopyWithoutKeywords(shaderOptions, material, group => ShaderAccessor.FilterByMask(group, (int)options.UpdateContents));
            }
        }

        public override void UpdateMateria(OceanCameraTask oceanCamera, MaterialData data, MaterialUpdateOptions options)
        {
            if (options.UpdateRenderQueue)
                data.Material.renderQueue = (int)ProjectSettings.Current.RenderQueue;

            int hashCode = GetHashCode();
            if (options.UpdateContents != 0 && (data.UpdaterHash != hashCode || data.Version != Version))
            {
                UpdateMateria(data.Material, options);
                data.UpdaterHash = hashCode;
                data.Version = Version;
            }
            else
            {
                if (shaderOptions.Cookie.Texture != null)
                {
                    data.Material.SetTexture(CookieOptions.TextureShaderFieldID, shaderOptions.Cookie.Texture.GetCurrentTexture());
                }
            }

            Matrix4x4 worldToLightMatrix;
            Light sunLight = oceanCamera.Data.SunLight;
            if (shaderOptions.Mode.Cookie != CookieMode.None)
            {
                var sunLightTransform = sunLight.transform;
                worldToLightMatrix = Matrix4x4.TRS(sunLightTransform.position, sunLightTransform.rotation, shaderOptions.Cookie.Scale).inverse;
            }
            else
            {
                worldToLightMatrix = Matrix4x4.identity;
            }
            data.Material.SetMatrix(CookieOptions.WorldToCookieMatrixShaderID, worldToLightMatrix);
        }

        public override PreparedContent GetRenderContents()
        {
            var modeOptions = shaderOptions.Mode;
            PreparedContent renderContents = PreparedContent.SunLight;

            if (modeOptions.Clip != ClipMode.None)
            {
                renderContents |= PreparedContent.ClipTexture;
            }


            if (modeOptions.Refraction == RefractionMode.Simple)
            {
                renderContents |= PreparedContent.RefractionTexture | PreparedContent.RefractionDepthTexture;
            }
            else if (modeOptions.Refraction == RefractionMode.Full)
            {
                renderContents |= PreparedContent.RefractionTexture | PreparedContent.RefractionDepthTexture | PreparedContent.UnderOceanMarkTexture;
            }


            if (modeOptions.BackRefraction == BackRefractionMode.Simple)
            {
                renderContents |= PreparedContent.RefractionTexture | PreparedContent.RefractionDepthTexture;
            }
            else if (modeOptions.BackRefraction == BackRefractionMode.Full)
            {
                renderContents |= PreparedContent.RefractionTexture | PreparedContent.RefractionDepthTexture | PreparedContent.UnderOceanMarkTexture;
            }


            if (modeOptions.FoamShpere == FoamShpereMode.Shpere8)
            {
                renderContents |= PreparedContent.RefractionDepthTexture | PreparedContent.Foam8;
            }
            else if (modeOptions.FoamShpere == FoamShpereMode.Shpere16)
            {
                renderContents |= PreparedContent.RefractionDepthTexture | PreparedContent.Foam16;
            }
            else if (modeOptions.Foam != FoamMode.None)
            {
                renderContents |= PreparedContent.RefractionDepthTexture;
            }


            if (modeOptions.FoamArea == FoamAreaMode.Area1)
            {
                renderContents |= PreparedContent.FoamArea1;
            }


            if (modeOptions.Reflection != ReflectionMode.None)
            {
                renderContents |= PreparedContent.ReflectionTexture;
            }


            if (modeOptions.Ripple != RippleMode.None)
            {
                renderContents |= PreparedContent.Ripple;
            }

            return renderContents;
        }

        [ContextMenu(nameof(PasteWithoutMode))]
        private void PasteWithoutMode()
        {
            OceanModeOptions old = null;
            Paste(this, delegate ()
            {
                old = shaderOptions.Mode.Clone();
            },
            delegate ()
            {
                shaderOptions.Mode = old;
            });
        }
    }
}
