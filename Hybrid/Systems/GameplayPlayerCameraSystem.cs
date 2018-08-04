using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Derby.Systems {
    
    public class GameplayPlayerCameraSystem : ComponentSystem {
        
        private struct CameraComponents : IComponentData {
            [ReadOnly] public SharedComponentDataArray<CameraInstance> cameras;
            public ComponentDataArray<Position> positions;
            public ComponentDataArray<Rotation> rotations;
            public ComponentDataArray<IntId> ids;
            public readonly int Length;
        }

        private struct VehicleComponents : IComponentData {
            [ReadOnly] public SharedComponentDataArray<RigidbodyInstance> rbodies;
            public ComponentDataArray<PlayerCamera> data;
            public ComponentDataArray<Position> positions;
            public ComponentDataArray<Rotation> rotations;
            public ComponentDataArray<IntId> ids;
            public readonly int Length;
        }

        [Inject]
        private CameraComponents cameraComponents;

        [Inject]
        private VehicleComponents vehicleComponents;

        protected override void OnUpdate() {
            for (int i = 0; i < cameraComponents.Length; i++) {
                var id = cameraComponents.ids[i].value;
                UpdateCameraTransform(i, id);
            }
        }

        private void UpdateCameraTransform(int i, int id) {
            var delta = Time.deltaTime;
            var translationSpeed = vehicleComponents.data[id].translationSpeed;
            var rotationSpeed = vehicleComponents.data[id].rotationSpeed;
            var lookAheadFactor = vehicleComponents.data[id].lookAheadFactor;

            var vehiclePosition = vehicleComponents.positions[id].Value;

            // Normalize the vehicle velocity
            Vector3 vehicleVelocity = vehicleComponents.rbodies[id].rigidbody.velocity;
            vehicleVelocity.y = 0;

            var velocity = new float3(vehicleVelocity.normalized);

            var currentCameraPosition = cameraComponents.positions[i].Value;
            var targettedCameraPosition = (-velocity * vehicleComponents.data[id].minDistance) + vehiclePosition - velocity * lookAheadFactor;
            var cameraLookAtPosition = vehiclePosition + velocity;

            targettedCameraPosition.y += 5f;

            var forward = math.normalize(cameraLookAtPosition - targettedCameraPosition);
            var rotation = quaternion.lookRotation(forward, new float3(0, 1, 0));

            cameraComponents.rotations[i] = new Rotation { Value = math.slerp(cameraComponents.rotations[i].Value, rotation, delta * rotationSpeed) };
            cameraComponents.positions[i] = new Position { Value = math.lerp(currentCameraPosition, targettedCameraPosition, Time.deltaTime * translationSpeed) };
        }
    }
}
