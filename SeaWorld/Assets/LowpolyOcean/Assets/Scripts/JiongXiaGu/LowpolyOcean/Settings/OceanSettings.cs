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
    public abstract class OceanSettings : ScriptableObjectEX
    {
        protected OceanSettings()
        {
        }

        public abstract float GetVertexHeight(Vector3 worldPos);
        public abstract void UpdateMateria(Material material, MaterialUpdateOptions options);
        public abstract void UpdateMateria(OceanCameraTask oceanCamera, MaterialData data, MaterialUpdateOptions options);
        public abstract PreparedContent GetRenderContents();
    }
}
