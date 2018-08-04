using CommonStructures;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Derby.Systems {

    public class CheckpointDetectionSystem : JobComponentSystem {

        private struct Checkpoints : IComponentData {
            [ReadOnly] public SharedComponentDataArray<CheckpointDetection> checks;
            [ReadOnly] public ComponentDataArray<IntId> checkIds;
            public readonly int Length;
        }

        private struct RaycastSetUp : IJob {

            [ReadOnly]
            public SharedComponentDataArray<CheckpointDetection> checkpoints;

            public NativeArray<RaycastCommand> commands;

            public void Execute() {
                for (int i = 0, j = 0; i < checkpoints.Length; i++) {
                    var raycastPoints = checkpoints[i].values;
                    var matrix = checkpoints[i].matrix;
                    for (int k = 0; k < raycastPoints.Length; k++, j++) {
                        var lhs = matrix.MultiplyPoint3x4(raycastPoints[k].lhs);
                        var rhs = matrix.MultiplyPoint3x4(raycastPoints[k].rhs);

                        var index = j * 2;

                        commands[index] = new RaycastCommand(lhs, rhs - lhs);
                        commands[index + 1] = new RaycastCommand(rhs, lhs - rhs);
                    }
                }
            }
        }

        [Inject]
        private Checkpoints data;

        private void TryUpdateCheckpointLap(int playerId, int checkpointId) {
            var lapTracker = DerbyGameplayCheckpointBootstrap.LapTracker;
            var current = lapTracker[playerId];

            // If the checkpoint is the next checkpoint...
            if (((current.item1 + 1) % lapTracker.CheckpointLength) == checkpointId) {
                current = Tuple<int, int>.CreateTuple(checkpointId, current.item2);
#if UNITY_EDITOR
                Debug.LogFormat("<color=#00ff00ff>Successfully passed checkpoint: {0}</color>", checkpointId);
#endif
                
                if (checkpointId == lapTracker.CheckpointLength - 1 && lapTracker.IsLapLocked) {
                    lapTracker.IsLapLocked = false;
                    lapTracker[playerId] = Tuple<int, int>.CreateTuple(checkpointId, current.item2);
#if UNITY_EDITOR
                    Debug.LogFormat("<color=#008080ff>Successfully unlocked the checkpoint: 0 for lap #{0}.</color>", current.item2);
#endif
                }
            }

            // Increment the lap count of the circuit if we passed the original point again and lock the write data.
            // To unlock the write data you have to move to the next checkpoint.
            if (current.item1 == 0 && !lapTracker.IsLapLocked) {
                // Lock the write data
                lapTracker.IsLapLocked = true;
                lapTracker[playerId] = Tuple<int, int>.CreateTuple(checkpointId, current.item2 + 1);

#if UNITY_EDITOR
                Debug.LogFormat("<color=#ffff00ff>Successfully completed lap: {0} and locked.</color>", lapTracker[playerId].item2);
#endif
            }
        }

        private void UpdatePlayerLap(NativeArray<RaycastHit> results) {
            var playerMap = DerbyGameplayBootStrap.PlayerColliderMap;
            for (int i = 0; i < results.Length; i++) {
                var collider = results[i].collider;
                int playerId;

                if (collider && playerMap.TryGetValue(collider, out playerId)) {
                    var checkpointId = i / 10;
                    TryUpdateCheckpointLap(playerId, checkpointId);
                }
            }
        }

        private JobHandle JobChain(JobHandle inputDeps) {
            var size = data.Length * data.checks[0].values.Length * 2;

            // Build the RaycastCommands
            var commands = new NativeArray<RaycastCommand>(size, Allocator.TempJob);

            // Output
            var results = new NativeArray<RaycastHit>(size, Allocator.TempJob);

            var raycastJob = new RaycastSetUp {
                checkpoints = data.checks,
                commands = commands
            };

            var deps = raycastJob.Schedule(inputDeps);
            deps.Complete();

            // Do the raycast job
            deps = RaycastCommand.ScheduleBatch(commands, results, 32, inputDeps);
            deps.Complete();

            UpdatePlayerLap(results);

            // Dispose the raycast commands as the results are needed.
            commands.Dispose();

            // Dispose the results, we've finished processing them.
            results.Dispose();

            return deps;
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps) {
            var deps = JobChain(inputDeps);

            return deps;
        }
    }
}
