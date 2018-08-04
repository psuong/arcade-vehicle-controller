using Unity.Entities;
using Unity.Mathematics;

namespace Derby.Systems {

    using InControl;

    public class GameplayInputHandler : ComponentSystem {

        private struct InputComponent : IComponentData {
            public ComponentDataArray<PlayerInput> inputs;
            public readonly int Length;
        }

        // The order of the array represents the player id. E.g: Index 0 represents 
        // player 0 as the device wrote into that element of the array.
        [Inject]
        private InputComponent players;

        private PlayerPool pool;

        protected override void OnStartRunning() {
            pool = DerbyGameplayBootStrap.PlayerPool;
        }

        protected override void OnUpdate () {
            for (int i = 0; i < players.Length; i++) {
                UpdatePlayerData(i);
            }
        }

        private void UpdatePlayerData (int i) {
            var device = InputManager.Devices[i];

            var playerIndex = pool[device.GUID];

            if (playerIndex >= 0) {

                var x = device.LeftStick.X;
                var y = device.RightTrigger.RawValue - device.LeftTrigger.RawValue;

                players.inputs[playerIndex] = new PlayerInput {
                    xy = new float2(x, y),
                    isBraking = device.Action3.IsPressed ? 1 : 0
                };
            }
#if UNITY_EDITOR
            else {
                UnityEngine.Debug.LogErrorFormat("Device with GUID: {0} is not registered in the Player Pool!", device.GUID);
            }
#endif
        }
    }
}
