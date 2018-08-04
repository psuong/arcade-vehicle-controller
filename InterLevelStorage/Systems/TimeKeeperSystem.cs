using UnityEngine;
using UnityEngine.Assertions;

namespace Derby {

    public class TimeKeeperSystem : MonoBehaviour {

        [SerializeField]
        private TimeTracker timeTracker;

        private void Awake() {
            Assert.IsNotNull(timeTracker, "No TimeTracker found!");
        }
    }
}
