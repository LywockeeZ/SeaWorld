using System;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    /// <summary>
    /// Mark under ocean area
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    [AddComponentMenu(EditorModeHelper.AddComponentMenuNameRoot + nameof(UnderOceanMarkDrawer))]
    public class UnderOceanMarkDrawer : MonoBehaviour, IObserver<CameraTaskEvent>
    {
        protected UnderOceanMarkDrawer()
        {
        }
 
        [SerializeField] private bool isEnabledMarkDraw;
        [SerializeField] private OceanExpandRender parent;
        [SerializeField] private float minOffset = -5f;
        [SerializeField] private float maxOffset = 5f;
        [SerializeField] private float waveHeight = 1f;
        [SerializeField] private Material underOceanMarkMat;
        private MeshFilter meshFilter;

        public float MinOffset
        {
            get { return minOffset; }
            set { minOffset = value; }
        }

        public float MaxOffset
        {
            get { return maxOffset; }
            set { maxOffset = value; }
        }

        public float WaveHeight
        {
            get { return waveHeight; }
            set { waveHeight = value; }
        }

        public Material UnderOceanMarkMaterial
        {
            get { return underOceanMarkMat; }
            set { underOceanMarkMat = value; }
        }

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        private void UpdateMarkPosition(Camera camera)
        {
            Vector3 cameraPosition = camera.transform.position;
            float oceanheight = parent.transform.position.y + waveHeight;

            var height = oceanheight - cameraPosition.y;
            height = Mathf.Clamp(height, minOffset, maxOffset);

            Vector3 pos = transform.localPosition;
            pos.y = height;
            transform.localPosition = pos;
        }

        private void OnWillRenderObject()
        {
            if (parent == null)
            {
                Debug.LogWarning("Undefined " + nameof(parent), this);
                return;
            }

            var camera = Camera.current;
            if (camera == null)
                return;

            OceanCameraTask oceanCamera = camera.GetComponent<OceanCameraTask>();
            if (oceanCamera != null)
            {
                if (underOceanMarkMat == null)
                {
                    Debug.LogError(new ArgumentNullException(nameof(underOceanMarkMat)), this);
                    return;
                }

                if (isEnabledMarkDraw)
                {
                    UpdateMarkPosition(camera);
                    oceanCamera.AddUnderUnderOceanMarkDrawer(this);
                }
            }
        }

        void IObserver<CameraTaskEvent>.OnCompleted()
        {
        }

        void IObserver<CameraTaskEvent>.OnError(Exception error)
        {
            Debug.LogError(error, this);
        }

        void IObserver<CameraTaskEvent>.OnNext(CameraTaskEvent value)
        {
            Matrix4x4 matrix = transform.ToMatrix();
            Graphics.DrawMesh(meshFilter.sharedMesh, matrix, underOceanMarkMat, value.Layer, value.RenderCamera, 0, null, false, false, false);
        }
    }
}
