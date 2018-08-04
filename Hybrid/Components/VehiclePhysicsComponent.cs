using System;
using Unity.Entities;

namespace Derby {

    [Serializable]
    public struct VehiclePhysics : IComponentData {
        public float hover, gravity, height;
    }

    public class VehiclePhysicsComponent : ComponentDataWrapper<VehiclePhysics> { }
}
