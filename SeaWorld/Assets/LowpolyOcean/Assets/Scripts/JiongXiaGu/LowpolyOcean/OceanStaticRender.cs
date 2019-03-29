using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// Manually specify ocean render content, provided to ocean that does not change often at runtime;
    /// Usually used in static and simple water;
    /// generally use with <see cref="OceanRenderTrigger"/>
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu(EditorModeHelper.AddComponentMenuNameRoot + nameof(OceanStaticRender))]
    public sealed class OceanStaticRender : OceanRender
    {
        private OceanStaticRender()
        {
        }

        [SerializeField] [EnumMask] private PreparedContent renderContents;

        public PreparedContent RenderContents
        {
            get { return renderContents; }
            set { renderContents = value; }
        }

        public override PreparedContent GetPreparedContents(OceanCameraTask oceanCamera)
        {
            return renderContents;
        }
    }
}
