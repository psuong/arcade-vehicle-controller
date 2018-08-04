using System;
using Unity.Entities;

namespace Derby {

    [Serializable]
    public struct VehicleMovement : IComponentData {
        public float thrust, turn;
    }

    public class VehicleMovementComponent : ComponentDataWrapper<VehicleMovement> { }
}
