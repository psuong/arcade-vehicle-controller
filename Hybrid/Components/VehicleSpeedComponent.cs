using System;
using Unity.Entities;
using UnityEngine;

namespace Derby {

    [Serializable]
    public struct VehicleSpeed : IComponentData {
        public float forwardSpeed, backwardSpeed, turnSpeed, drag;
    }

    public class VehicleSpeedComponent : ComponentDataWrapper<VehicleSpeed> { }
}
