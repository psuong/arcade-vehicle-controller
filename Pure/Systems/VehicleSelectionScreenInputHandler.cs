using Unity.Entities;

namespace Derby.Systems {

    public class VehicleSelectionScreenInputHandler : ComponentSystem {

        [Inject]
        private SelectionBrowseComponent selection;

        [Inject]
        private SelectionConfirmationComponent confirmation;

        protected override void OnUpdate() {
            for (int i = 0; i < selection.Length; i++) {
                UpdateInputData(i);
            }
        }

        private void UpdateInputData(int i) { 
            // TODO: Get a the device input or whatever input source you need.
            var deviceGUID = System.Guid.Empty;
            var index = VehicleSelectionScreenBootStrap.PlayerPool[device.GUID];

            if (index >= 0) {
                selection.input[index] = new SelectionBrowseData {
                    horizontal = Clamp(device.LeftStick.X),
                };

                confirmation.input[index] = new SelectionConfirmationData {
                    // TODO: Left the default as zero since this System used InControl for my game.
                    accept = 0,
                    cancel = 0,
                };
            }
        }

        private int Clamp(float i) {
            if (i > 0) {
                return 1;
            } else if (i < 0) {
                return -1;
            } else {
                return 0;
            }
        }
    }

}
