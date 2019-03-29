using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.LowpolyOcean
{


    public enum UnderFogMode
    {
        None,

        /// <summary>
        /// Only forward rendering is supported. See "LPUnderOceanLighting.cginc"
        /// </summary>
        InPass,
    }
}
