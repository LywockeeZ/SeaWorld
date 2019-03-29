using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// if you need to implement custom settings, you can inherit and override this class.
    /// </summary>
    public abstract class UnderOceanSettings : ScriptableObjectEX
    {
        protected UnderOceanSettings()
        {
        }

        public abstract PreparedContent OnPreOceanRender(OceanCameraTask oceanCamera, OceanRender ocean);
        public abstract void OnPostOceanRender(OceanCameraTask oceanCamera);
    }
}
