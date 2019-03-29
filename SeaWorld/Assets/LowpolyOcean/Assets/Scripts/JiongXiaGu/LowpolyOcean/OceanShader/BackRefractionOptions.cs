using JiongXiaGu.ShaderTools;
using System;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    [Serializable]
    [ShaderFieldGroup(OceanMode.BackRefractionSimple | OceanMode.BackRefractionFull)]
    public class BackRefractionOptions
    {
        public const string ClarityShaderName = "_OceanBackRefractionClarity";
        public const string OffsetShaderName = "_OceanBackRefractionOffset";

        public static BackRefractionOptions Default => new BackRefractionOptions()
        {
            Clarity = 0.3f,
            Offset = 1,
        };

        [ShaderField(ClarityShaderName)] [Range(0, 1)] public float Clarity;
        [ShaderField(OffsetShaderName)] [Range(-5, 5)] public float Offset;
    }
}
