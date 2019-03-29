using System;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// notify <see cref="LowpolyOcean.OceanRender"/> when will render, generally use in ocean instance
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    [AddComponentMenu(EditorModeHelper.AddComponentMenuNameRoot + nameof(OceanRenderTrigger))]
    public class OceanRenderTrigger : MonoBehaviour, IObserver<CameraTaskEvent>
    {
        protected OceanRenderTrigger()
        {
        }

        [SerializeField] protected OceanRender oceanRender;
        [SerializeField] protected Material underOceanMarkMat;
        public MeshRenderer MeshRenderer { get; protected set; }
        public MeshFilter MeshFilter { get; protected set; }

        public OceanRender OceanRender
        {
            get { return oceanRender; }
            set { oceanRender = value; }
        }

        protected virtual void Awake()
        {
            MeshFilter = GetComponent<MeshFilter>();
            MeshRenderer = GetComponent<MeshRenderer>();
        }

        protected virtual void OnWillRenderObject()
        {
            if (oceanRender == null)
            {
                Debug.LogWarning("Undefined " + nameof(oceanRender), this);
                return;
            }

            var oceanCamera = oceanRender.OnWillRenderOcean();
            if (underOceanMarkMat != null && oceanCamera != null)
            {
                if (MeshFilter.sharedMesh != null)
                {
                    oceanCamera.AddUnderUnderOceanMarkDrawer(this);
                }
            }
        }

        void IObserver<CameraTaskEvent>.OnCompleted()
        {
        }

        void IObserver<CameraTaskEvent>.OnError(Exception error)
        {
            Debug.Log(error, this);
        }

        void IObserver<CameraTaskEvent>.OnNext(CameraTaskEvent value)
        {
            Matrix4x4 matrix = transform.ToMatrix();
            Graphics.DrawMesh(MeshFilter.sharedMesh, matrix, underOceanMarkMat, value.Layer, value.RenderCamera, 0, null, false, false, false);
        }
    }
}
