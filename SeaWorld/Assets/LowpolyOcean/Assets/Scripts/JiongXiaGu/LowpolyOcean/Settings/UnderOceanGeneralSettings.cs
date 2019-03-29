using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// under ocean info settings, need to ensure that the <see cref="SendValuesChanged"/> is called after the parameter has been modified;
    /// </summary>
    [CreateAssetMenu(menuName = EditorModeHelper.AssetMenuNameRoot + nameof(UnderOceanGeneralSettings))]
    public class UnderOceanGeneralSettings : UnderOceanSettings
    {
        protected UnderOceanGeneralSettings()
        {
        }

        [SerializeField] protected UnderOceanShaderOptions shaderOptions;

        public UnderOceanShaderOptions ShaderOptions
        {
            get { return shaderOptions; }
            set { shaderOptions = value; }
        }

        public override PreparedContent OnPreOceanRender(OceanCameraTask oceanCamera, OceanRender ocean)
        {
            var sunLight = oceanCamera.Data.SunLight;
            if (sunLight == null)
            {
                Debug.LogError(new ArgumentNullException(nameof(sunLight)), this);
                return PreparedContent.None;
            }


            Matrix4x4 worldToLightMatrix;
            if (ShaderOptions.Mode.Cookie > 0)
            {
                var sunLightTransform = sunLight.transform;
                worldToLightMatrix = Matrix4x4.TRS(sunLightTransform.position, sunLightTransform.rotation, shaderOptions.Cookie.Scale).inverse;
            }
            else
            {
                worldToLightMatrix = Matrix4x4.identity;
            }
            Shader.SetGlobalMatrix(UnderCookieOptions.WorldToCookieMatrixShaderID, worldToLightMatrix);


            if (!UnderOceanShaderOptions.UpdateShaderFields(shaderOptions))
            {
                if (shaderOptions.Cookie.Texture != null)
                {
                    Shader.SetGlobalTexture(UnderCookieOptions.TextureShaderFieldID, shaderOptions.Cookie.Texture.GetCurrentTexture());
                }
            }

            if (ProjectSettings.Current.RenderQueue == OceanRenderQueue.Transparent)
            {
                UnderOceanShaderOptions.DisableUnderOceanFogEffect();
            }

            Shader.SetGlobalVector(UnderOceanShaderOptions.UnderOceanPositionShaderID, ocean.transform.position);

            return PreparedContent.UnderOceanMarkTexture;
        }

        public override void OnPostOceanRender(OceanCameraTask oceanCamera)
        {
        }

        [ContextMenu(nameof(SendValuesChanged))]
        public virtual void SendValuesChanged()
        {
            shaderOptions.Dirty();
        }

        protected virtual void OnValidate()
        {
            SendValuesChanged();
        }
    }
}
