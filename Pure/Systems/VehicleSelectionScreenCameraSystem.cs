using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Derby.Systems {

    public class VehicleSelectionScreenCameraSystem : JobComponentSystem {

        private struct SelectionScreenCameras : IJobProcessComponentData<Position, CameraTracker, SelectionBrowseData> {

            public readonly float Delta;
            // TODO: Change this into a native array?
            public readonly Vector3[] Positions;

            public void Execute(ref Position position, ref CameraTracker tracker, ref SelectionBrowseData browse) {
                if (tracker.isLocked == 0) {
                    var size = Positions.Length;

                    if (browse.horizontal == 1 && tracker.isMoving == 0) {
                        tracker.isMoving = 1;
                        tracker.index = (tracker.index + 1) % size;
                        tracker.target = Positions[tracker.index];
                    }

                    if (browse.horizontal == -1 && tracker.isMoving == 0) {
                        tracker.isMoving = 1;
                        tracker.index = (tracker.index <= 0) ? size - 1 : (tracker.index - 1) % size;
                        tracker.target = Positions[tracker.index];
                    }

                    if (tracker.isMoving == 1) {
                        tracker.t += Delta;
                        position.Value = math.lerp(position.Value, tracker.target, tracker.t);

                        if (tracker.t >= 1) {
                            tracker.t = 0;
                            tracker.isMoving = 0;
                        }
                    }
                }
            }

            public SelectionScreenCameras(float delta, Vector3[] positions) {
                Delta = delta;
                Positions = positions;
            }
        }


        protected override JobHandle OnUpdate(JobHandle inputDeps) {

            var job = new SelectionScreenCameras(
                Time.deltaTime, 
                PositionSelectionWrapper.PositionSelection.points
            );

            return job.Schedule(this, 64, inputDeps);
        }
    }
}
