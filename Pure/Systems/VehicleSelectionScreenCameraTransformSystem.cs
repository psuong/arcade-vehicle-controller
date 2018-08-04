using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Derby.Systems {

    [UpdateAfter(typeof(VehicleSelectionScreenCameraSystem))]
    public class VehicleSelectionScreenCameraTransformSystem : ComponentSystem {

        private struct Cameras : ISharedComponentData {
            [ReadOnly]
            public SharedComponentDataArray<CameraInstance> instances;
            public readonly int Length;
        }

        private struct CameraPositions : IComponentData {
            public ComponentDataArray<Position> values;
            public readonly int Length;
        }

        [Inject]
        private Cameras cameras;

        [Inject]
        private CameraPositions positions;

        protected override void OnUpdate() {
            // Move the cameras to their desired positions.
            for (int i = 0; i < cameras.Length; i++) {
                cameras.instances[i].camera.transform.position = positions.values[i].Value;
            }
        }
    }
}
