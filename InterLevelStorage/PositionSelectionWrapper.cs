using UnityEngine;
using UnityEngine.Assertions;

namespace Derby {

    public class PositionSelectionWrapper : MonoBehaviour {

        public static PositionSelection PositionSelection {
            get;
            private set;
        }
        
        public PositionSelection carPositions;

        private void Awake() {
            Assert.IsNotNull(carPositions, "No PositionSelection found!");
            PositionSelectionWrapper.PositionSelection = carPositions;
        }
    }
}
