using Unity.Entities;
using Unity.Mathematics;

namespace Derby.Systems {

    public class GameplayPlayerInputInterpreter : ComponentSystem {

        private struct PlayerInputComponent : IComponentData {
            public ComponentDataArray<FloatId> constraints;
            public ComponentDataArray<VehicleSpeed> speeds;
            public ComponentDataArray<VehicleMovement> movements;
            public ComponentDataArray<PlayerInput> inputs;
            public readonly int Length;
        }

        [Inject]
        private PlayerInputComponent interpretations;

        protected override void OnUpdate() {
            for (int i = 0; i < interpretations.Length; i++) {
                MoveAndTurn(i);
            }
        }

        private void MoveAndTurn(int i) {
            var deadZone = interpretations.constraints[i].value;
            var input = interpretations.inputs[i];
            var vector = interpretations.inputs[i].xy;

            float thrust = 0f, turn = 0f;
            if (input.isBraking != 1) {
                var speed = interpretations.speeds[i];
                if (vector.y > deadZone) {
                    thrust = vector.y * speed.forwardSpeed;
                } else if (vector.y < -deadZone) {
                    thrust = vector.y * speed.backwardSpeed;
                }
            }

            if (math.abs(vector.x) > deadZone) {
                turn = vector.x;
            }

            interpretations.movements[i] = new VehicleMovement {
                thrust = thrust,
                turn = turn
            };
        }
    }
}
