using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{
    public interface ICameraTaskData
    {
        PreparedContent PreparedContents { get; }
        CameraTaskReflectionData ReflectionData { get; }
        CameraTaskRippleData RippleData { get; }
        Light SunLight { get; }
        float OceanTime { get; }
    }
}
