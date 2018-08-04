using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Derby.Systems {
   
    [UpdateInGroup(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
    public class GameplayPlayerHoverSystem : ComponentSystem {

        private struct HoverComponents : IComponentData {
            [ReadOnly] public SharedComponentDataArray<HoverPoints> points;
            [ReadOnly] public SharedComponentDataArray<RigidbodyInstance> rbodies;
            public ComponentDataArray<VehicleSpeed> speeds;
            public ComponentDataArray<VehiclePhysics> physics;
            public ComponentDataArray<VehicleMovement> movements;

            public readonly int Length;
        }

        [Inject]
        private HoverComponents hoverData;
        
        protected override void OnUpdate() {
            for (int i = 0; i < hoverData.Length; i++) {
                Hover(i);
            }
        }

        private void Hover(int i) {
            var hPoints = hoverData.points[i].hoverPoints;
            var physics = hoverData.physics[i];
            var movement = hoverData.movements[i];
            var speed = hoverData.speeds[i];
            var rbody = hoverData.rbodies[i].rigidbody;

            for (int k = 0; k < hPoints.Length; k++) {
                var point = hPoints[k];
                
                bool isGrounded = false;
                RaycastHit hit;

                if (Physics.Raycast(point.position, -Vector3.up, out hit)) {
                    isGrounded = true;
                    rbody.AddForceAtPosition(Vector3.up * physics.hover * (1f - (hit.distance / physics.height)), point.position);
                } else {
                    rbody.AddForceAtPosition(point.up * ((rbody.transform.position.y > point.position.y) ? physics.gravity : -physics.gravity), point.position);
                }

                if (isGrounded) {
                    rbody.drag = speed.drag;
                } else {
                    rbody.drag = 0f;
                    hoverData.movements[i] = new VehicleMovement {
                        turn = movement.turn,
                        thrust = movement.thrust / 5f
                    };
                }
            }
        }
    }
}
