using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Derby.Systems {
    
    [UpdateAfter(typeof(TransformSystem))]
    public class ScaleSystem : JobComponentSystem {

        private struct ScaleJob : IJobParallelFor {
            [ReadOnly] public ComponentDataArray<Scale> scales;
            public ComponentDataArray<TransformMatrix> matrices;

            public void Execute(int i) {
                var scale = scales[i].Value;
                matrices[i] = new TransformMatrix { Value = math.mul(matrices[i].Value, new float4x4(scale.x, 0, 0, 0, 0, scale.y, 0, 0, 0, 0, scale.z, 0, 0, 0, 0, 1)) };
            }
        }

        private struct Transforms : IComponentData {
            public ComponentDataArray<Scale> scales;
            public ComponentDataArray<TransformMatrix> matrices;
            public readonly int Length;
        }

        [Inject]
        private Transforms transforms;

        protected override JobHandle OnUpdate(JobHandle inputDeps) {
            var job = new ScaleJob {
                scales = transforms.scales,
                matrices = transforms.matrices
            };
            
            return job.Schedule(transforms.Length, 32);
        }
    }
}
