using Unity.Entities;
using Unity.Mathematics;

namespace Derby {
    
    // TODO: Remove this?
    /// <summary>
    /// Store all of the positions.
    /// </summary>
    public struct CarPositions : ISharedComponentData {
        public float3[] positions;
    }

    public class CarPositionsComponent : SharedComponentDataWrapper<CarPositions> { }

}
