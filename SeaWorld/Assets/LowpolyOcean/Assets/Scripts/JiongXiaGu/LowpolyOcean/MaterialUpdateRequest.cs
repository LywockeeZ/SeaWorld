using System;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    [Serializable]
    public struct MaterialUpdateOptions
    {        
        /// <summary>
        /// update material keywords
        /// </summary>
        public bool UpdateKeyword;

        /// <summary>
        /// update material render queue
        /// </summary>
        public bool UpdateRenderQueue;

        [EnumMask] public OceanMode UpdateContents;

        public MaterialUpdateOptions(bool updateKeyword, bool updateRenderQueue, OceanMode updateContents)
        {
            UpdateKeyword = updateKeyword;
            UpdateRenderQueue = updateRenderQueue;
            UpdateContents = updateContents;
        }
    }

    [Serializable]
    public class MaterialUpdateRequest
    {
        public string Name;
        public bool Enable;

        /// <summary>
        /// update material keywords
        /// </summary>
        public bool UpdateKeyword;

        /// <summary>
        /// update material render queue
        /// </summary>
        public bool UpdateRenderQueue;

        [EnumMask] public OceanMode UpdateContents = (OceanMode)~0;
        public OceanSettings OceanSettings;
        public MaterialData Material;
    }
}
