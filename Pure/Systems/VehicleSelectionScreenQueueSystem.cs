using Unity.Entities;

namespace Derby.Systems {

    [UpdateAfter(typeof(VehicleSelectionScreenCameraSystem))]
    public class VehicleSelectionScreenQueueSystem : ComponentSystem {

        private struct ConfirmationData : IComponentData {
            public ComponentDataArray<SelectionConfirmationData> confirmations;
            public ComponentDataArray<IntId> ints;
            public ComponentDataArray<CameraTracker> trackers;
            public readonly int Length;
        }

        [Inject]
        private ConfirmationData data;

        protected override void OnUpdate() {
            for (int i = 0; i < data.Length; i++) {
                TryQueue(i);
            }
        }

        private void TryQueue(int i) {
            var id = data.ints[i].value;

            if (id >= 0) {
                var accept = data.confirmations[i].accept;
                var cancel = data.confirmations[i].cancel;

                if (accept == 1) {
                    VehicleSelectionScreenBootStrap.VehicleSelectionCache[id] = data.trackers[i].index;
                    TryLock(i, 1);
                }

                if (cancel == 1) {
                    VehicleSelectionScreenBootStrap.VehicleSelectionCache[id] = -1;
                    TryLock(i, 0);
                }
            }
        }

        private void TryLock(int i, int state) {
            var original = data.trackers[i];
            if (original.isMoving == 0) {
                data.trackers[i] = new CameraTracker{
                    index = original.index,
                    isMoving = original.isMoving,
                    isLocked = state,
                    t = original.t,
                    target = original.target
                };
            }
#if UNITY_EDITOR
            else {
                UnityEngine.Debug.LogWarning("The camera could not lock until the camera stops moving!");
            }
#endif
        }
    }
}
