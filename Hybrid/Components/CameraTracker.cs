using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Derby {

    /// <summary>
    /// Stores and keeps track of the camera's position and state when browsing positions.
    /// </summary>
    [Serializable]
    public struct CameraTracker : IComponentData {
        public int index, isMoving, isLocked;
        public float t;
        public float3 target;
    }

    public class CameraTrackerComponent : ComponentDataWrapper<CameraTracker> { }
}
