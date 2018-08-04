using UnityEngine;

namespace Derby {
    
    /// <summary>
    /// Stores the progress of systems loading between scenes.
    /// </summary>
    [CreateAssetMenu(menuName = "Derby/Progress Cache", fileName = "Progress Cache")]
    public class ProgressCache : ScriptableObject {

        public float Progress {
            get { return progress; }
        }

        [SerializeField, Tooltip("What is the progress of whatever is loading?")]
        private float progress;

        /// <summary>
        /// Sets the progress value.
        /// </summary>
        public void UpdateProgress(float t) {
            progress = t;
        }
    
        /// <summary>
        /// Resets the value of the progress back down to zero.
        /// </summary>
        public void Reset() {
            progress = 0;
        }
    }
}
