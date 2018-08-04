using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Derby.Systems {
    
    // TODO: Support this as a job system in the future
    [UpdateInGroup(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate)), UpdateAfter(typeof(GameplayPlayerHoverSystem))]
    public class GameplayPlayerPhysicsMoveSystem : ComponentSystem {

        private struct PhysicsMovementComponent : ISharedComponentData {
            [ReadOnly] public SharedComponentDataArray<RigidbodyInstance> rigidbodies;
            public ComponentDataArray<VehiclePhysics> physics;
            public ComponentDataArray<VehicleMovement> movements;
            public ComponentDataArray<VehicleSpeed> speeds;
            public readonly int Length;
        }

        [Inject, ReadOnly]
        private PhysicsMovementComponent dependencies;
        
        protected override void OnUpdate() {
            for (int i = 0; i < dependencies.Length; i++) {
                Move(i);
                Turn(i);
            }
        }

        private void Move(int i) {
            var movement = dependencies.movements[i];

            if (math.abs(movement.thrust) > 0) {
                var rigidbody = dependencies.rigidbodies[i].rigidbody;
                var forward = rigidbody.transform.forward;

                rigidbody.AddForce(forward * movement.thrust);
            }
        }

        private void Turn(int i) {
            var movement = dependencies.movements[i];
            var rigidbody = dependencies.rigidbodies[i].rigidbody;
            var speed = dependencies.speeds[i];

            if (movement.turn> 0) {
                rigidbody.AddRelativeTorque(Vector3.up * movement.turn * speed.turnSpeed);
            } else if (movement.turn < 0) {
                rigidbody.AddRelativeTorque(Vector3.up * movement.turn * speed.turnSpeed);
            }
        }
    }
}
