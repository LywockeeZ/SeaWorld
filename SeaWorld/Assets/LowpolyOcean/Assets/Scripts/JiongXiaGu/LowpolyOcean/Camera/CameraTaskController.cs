using System;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// Work before ocean camera rendering;
    /// </summary>
    public class CameraTaskController : ICameraTaskController, IDisposable
    {
        public FoamCameraData FoamData { get; protected set; }
        public CameraTaskReflectionData ReflectionData { get; protected set; }
        public CameraTaskRippleData RippleData { get; protected set; }
        public PreparedContent PreparedContents { get; protected set; }
        public Light SunLight { get; private set; }
        public virtual float OceanTime => VertexHelper.DefaultTime;
        public bool IsHasUnderOceanEffect { get; private set; }
        private IDisposable underOceanFogDisposer;
        private IDisposable underOceanEffectDisposer;

        public CameraTaskController(OceanCameraTask oceanCamera)
        {
            FoamData = new FoamCameraData(oceanCamera.ThisCamera);
        }

        public void Dispose()
        {
            if (FoamData != null)
            {
                FoamData.Dispose();
                FoamData = null;
            }
        }

        protected virtual Light GetSunlight()
        {
            return RenderSettings.sun;
        }

        protected virtual void CollectOceanInfos(OceanCameraTask oceanCamera)
        {
            for (int i = 0; i < oceanCamera.WillRenderOceans.Count; i++)
            {
                var ocean = oceanCamera.WillRenderOceans[i];
                var current = ocean.GetPreparedContents(oceanCamera);

                if ((current & PreparedContent.ReflectionTexture) != 0)
                {
                    ReflectionData = ocean.GetReflectionData();
                }
                if ((current & PreparedContent.Ripple) != 0)
                {
                    RippleData = ocean.GetRippleData();
                }

                PreparedContents |= current;
            }
        }

        protected virtual void CollectUnderOceanInfos(OceanCameraTask oceanCamera)
        {
            if (oceanCamera.EnableUnderOceanEffect)
            {
                foreach (var ocean in OceanRender.ActiveOceans)
                {
                    if (ocean.IsEnableUnderOcean)
                    {
                        if (ocean.UnderOceanSettings == null)
                        {
                            Debug.LogError("The switch is turned on, but the subsea setting is not defined underOceanSettings", ocean);
                            continue;
                        }

                        PreparedContents |= ocean.UnderOceanSettings.OnPreOceanRender(oceanCamera, ocean);
                        IsHasUnderOceanEffect = true;
                        return;
                    }
                }
            }

            UnderOceanShaderOptions.DisableUnderOceanEffectAll();
        }

        public void OnPreOceanCull(OceanCameraTask oceanCamera)
        {
            FoamShpereSystem.Current.OnPreOceanCull(FoamData);
        }

        public virtual void OnPreOceanRender(OceanCameraTask oceanCamera)
        {
            SunLight = GetSunlight();
            CollectOceanInfos(oceanCamera);
            CollectUnderOceanInfos(oceanCamera);

            Shader.SetGlobalFloat(OceanShaderOptions.TimeShaderID, OceanTime);

            if ((PreparedContents & PreparedContent.SunLight) != 0 && SunLight != null)
            {
                Vector4 pos = SunLight.transform.eulerAngles;
                pos.w = Vector3.Dot(new Vector3(0, -1, 0), SunLight.transform.forward);
                Shader.SetGlobalVector(OceanShaderOptions.SunLightShaderID, pos);
                Shader.SetGlobalColor(OceanShaderOptions.SunLightColorShaderID, SunLight.color);
            }

            if ((PreparedContents & PreparedContent.Ripple) != 0)
            {
                RippleSystem.Current.SetValueToShader(oceanCamera, RippleData);
            }

            FoamShpereSystem.Current.SetValueToShader(oceanCamera, FoamData, PreparedContents);
            FoamAreaSystem.Current.SetValueToShader(oceanCamera, PreparedContents);
        }

        protected virtual void Rest()
        {
            PreparedContents = PreparedContent.None;
            IsHasUnderOceanEffect = false;
            SunLight = null;
        }

        public virtual void OnPostOceanRender(OceanCameraTask oceanCamera)
        {
            Rest();
        }

        public void OnPreRefractionRender(OceanCameraTask oceanCamera)
        {
            if (IsHasUnderOceanEffect)
            {
                underOceanFogDisposer = new UnderOceanShaderOptions.DisableUnderOceanFogScope();
            }
        }

        public void OnPostRefractionRender(OceanCameraTask oceanCamera)
        {
            if (underOceanFogDisposer != null)
            {
                underOceanFogDisposer.Dispose();
                underOceanFogDisposer = null;
            }
        }

        public void OnPreReflectionRender(OceanCameraTask oceanCamera)
        {
            if (IsHasUnderOceanEffect)
            {
                underOceanEffectDisposer = new UnderOceanShaderOptions.DisableUnderOceanEffectScope();
            }
        }

        public void OnPostReflectionRender(OceanCameraTask oceanCamera)
        {
            if (underOceanEffectDisposer != null)
            {
                underOceanEffectDisposer.Dispose();
                underOceanEffectDisposer = null;
            }
        }
    }
}
