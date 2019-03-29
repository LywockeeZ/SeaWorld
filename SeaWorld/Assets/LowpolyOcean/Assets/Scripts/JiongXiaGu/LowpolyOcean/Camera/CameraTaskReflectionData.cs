using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    public struct CameraTaskReflectionData
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }

        public override string ToString()
        {
            return string.Format("Postion:{0}, Normal:{1}", Position, Normal);
        }
    }
}
