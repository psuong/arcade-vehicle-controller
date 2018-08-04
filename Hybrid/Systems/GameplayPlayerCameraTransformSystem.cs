using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Derby.Systems {
    
    [UpdateAfter(typeof(GameplayPlayerCameraSystem))]
    public class GameplayPlayerCameraTransformSystem : ComponentSystem {

        private struct CameraComponents : IComponentData {
            [ReadOnly] public SharedComponentDataArray<CameraInstance> cameras;
            public ComponentDataArray<Position> positions;
            public ComponentDataArray<Rotation> rotations;
            public readonly int Length;
        }

        [Inject]
        private CameraComponents cameraComponents;

        protected override void OnUpdate() {
            for (int i = 0; i < cameraComponents.Length; i++) {
                cameraComponents.cameras[i].camera.transform.position = cameraComponents.positions[i].Value;
                cameraComponents.cameras[i].camera.transform.rotation = cameraComponents.rotations[i].Value;
            }
        }
    }
}
