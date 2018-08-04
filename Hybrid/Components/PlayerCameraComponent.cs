using System;
using Unity.Entities;

namespace Derby {

    [Serializable]
    public struct PlayerCamera : IComponentData {
        public float translationSpeed, rotationSpeed, minDistance, lookAheadFactor;
    }

    public class PlayerCameraComponent : ComponentDataWrapper<PlayerCamera> { }
}
