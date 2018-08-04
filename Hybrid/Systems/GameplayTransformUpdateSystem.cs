using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Derby.Systems {
    
    [UpdateAfter(typeof(GameplayPlayerPhysicsMoveSystem))]
    public class GameplayTransformUpdateSystem : ComponentSystem {

        private struct TransformComponents : IComponentData {
            [ReadOnly] public SharedComponentDataArray<RigidbodyInstance> rbodies;
            public ComponentDataArray<Position> positions;
            public ComponentDataArray<Rotation> rotations;
            public readonly int Length;
        }
        
        [Inject]
        private TransformComponents transforms;

        protected override void OnUpdate() {
            for (int i = 0; i < transforms.Length; i++) {
                UpdatePositionAndRotation(i);
            }
        }

        private void UpdatePositionAndRotation(int i) {
            var transform = transforms.rbodies[i].rigidbody.transform;
            transforms.positions[i] = new Position { Value = transform.position };
            transforms.rotations[i] = new Rotation { Value = transform.rotation };
        }
    }
}
