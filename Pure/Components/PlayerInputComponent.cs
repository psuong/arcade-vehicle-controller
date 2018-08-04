using Unity.Entities;
using Unity.Mathematics;

namespace Derby {

    /// <summary>
    /// Stores input data found from controllers and keyboards.
    /// </summary>
    public struct PlayerInput : IComponentData {
        public float2 xy;
        public int isBraking; // TODO: Remove this?
    }

    public class PlayerInputComponent : ComponentDataWrapper<PlayerInput> { }
}
