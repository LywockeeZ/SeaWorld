using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// provide ocean information to camera through <see cref="OceanSettings"/>;
    /// generally use with <see cref="OceanRenderTrigger"/>
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu(EditorModeHelper.AddComponentMenuNameRoot + nameof(OceanExpandRender))]
    public sealed class OceanExpandRender : OceanRender
    {
        private OceanExpandRender()
        {
        }

        [SerializeField] private OceanObserverList oceanObservers;
        public IList<MaterialUpdateRequest> OceanObservers => oceanObservers;

        [ContextMenu(nameof(UpdateObservers))]
        public void UpdateObservers()
        {
            foreach (var observer in oceanObservers.List)
            {
                if (observer.Enable && observer.Material.Material != null && observer.OceanSettings != null)
                    observer.OceanSettings.UpdateMateria(observer.Material, new MaterialUpdateOptions(observer.UpdateKeyword, observer.UpdateRenderQueue, observer.UpdateContents));
            }
        }

        private void UpdateObservers(OceanCameraTask oceanCamera)
        {
            foreach (var observer in oceanObservers.List)
            {
                if(observer.Enable && observer.Material.Material != null && observer.OceanSettings != null)
                    observer.OceanSettings.UpdateMateria(oceanCamera, observer.Material, new MaterialUpdateOptions(observer.UpdateKeyword, observer.UpdateRenderQueue, observer.UpdateContents));
            }
        }

        public override PreparedContent GetPreparedContents(OceanCameraTask oceanCamera)
        {
            UpdateObservers(oceanCamera);
            PreparedContent contents = PreparedContent.None;
            foreach (var observer in oceanObservers.List)
            {
                if(observer.Enable && observer.OceanSettings != null)
                    contents |= observer.OceanSettings.GetRenderContents();
            }
            return contents;
        }

        [Serializable]
        public class OceanObserverList : CustomReorderableList<MaterialUpdateRequest>
        {
        }
    }
}
