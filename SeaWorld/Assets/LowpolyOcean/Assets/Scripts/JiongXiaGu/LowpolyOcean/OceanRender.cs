using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// ocean render base class, provide scene ocean instance and render content to <see cref="OceanCameraTask"/>;
    /// generally use with <see cref="OceanRenderTrigger"/>;
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public abstract class OceanRender : MonoBehaviour
    {
        protected OceanRender()
        {
        }

        private static readonly List<OceanRender> activeOceans = new List<OceanRender>();
        public static IReadOnlyCollection<OceanRender> ActiveOceans => activeOceans;

        [SerializeField] private bool isEnableUnderOcean;
        [SerializeField] private UnderOceanSettings underOceanSettings;

        private bool isRending;
        private Camera requestCamera;
        private OceanCameraTask requestOceanCamera;

        public bool IsEnableUnderOcean
        {
            get { return isEnableUnderOcean; }
            set { isEnableUnderOcean = value; }
        }

        public UnderOceanSettings UnderOceanSettings
        {
            get { return underOceanSettings; }
            set { underOceanSettings = value; }
        }

        protected virtual void OnEnable()
        {
            activeOceans.Add(this);
        }

        protected virtual void OnDisable()
        {
            activeOceans.Remove(this);
        }

        public abstract PreparedContent GetPreparedContents(OceanCameraTask oceanCamera);

        public virtual OceanCameraTask OnWillRenderOcean()
        {
            if (isRending)
                return requestOceanCamera;

            var camera = Camera.current;
            if (camera == null)
                return null;

            OceanCameraTask oceanCamera;
            if (RenderHelper.TryGetOceanCamera(camera, out oceanCamera))
            {
                isRending = true;
                requestCamera = camera;
                requestOceanCamera = oceanCamera;

                oceanCamera.AddWillRenderOcean(this);
                return oceanCamera;
            }

            return null;
        }

        protected virtual void OnRenderObject()
        {
            if (Camera.current == requestCamera)
            {
                isRending = false;
                requestCamera = null;
                requestOceanCamera = null;
            }
        }

        public virtual CameraTaskReflectionData GetReflectionData()
        {
            return new CameraTaskReflectionData()
            {
                Position = transform.position,
                Normal = transform.up,
            };
        }

        public virtual CameraTaskRippleData GetRippleData()
        {
            return new CameraTaskRippleData()
            {
                OceanHeight = transform.position.y,
            };
        }

        /// <summary>
        /// Assign children's <see cref="OceanRenderTrigger.OceanRender"/> to this instance
        /// </summary>
        [ContextMenu(nameof(AssignChildTrigger))]
        public OceanRenderTrigger[] AssignChildTrigger()
        {
            var children = GetComponentsInChildren<OceanRenderTrigger>();
            foreach (var child in children)
            {
                child.OceanRender = this;
            }
            return children;
        }
    }
}
