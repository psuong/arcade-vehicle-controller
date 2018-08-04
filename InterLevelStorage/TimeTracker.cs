using UnityEngine;

namespace Derby {

    [CreateAssetMenu(menuName = "Derby/Time Tracker", fileName = "Time Tracker")]
    public class TimeTracker : ScriptableObject {

        public float[] times;

        public void Initialize(int size) {
            times = new float[size];
        }
    }
}
