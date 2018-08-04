using Unity.Entities;
using Unity.Mathematics;

namespace Derby {

    [System.Serializable]
    public struct Scale : IComponentData {
        public float3 Value;
    }

    public class ScaleComponent : ComponentDataWrapper<Scale> { }
}
