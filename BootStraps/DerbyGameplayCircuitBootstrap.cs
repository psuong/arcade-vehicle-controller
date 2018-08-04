using System.Collections;
using UnityEngine;

namespace Derby {

    /// <summary>
    /// A void delegate for handling any functions when the circuit starts.
    /// </summary>
    public delegate void CircuitStartHandler();

    /// <summary>
    /// A void delegate for handling any functions when the circuit is waiting throughout the delay.
    /// </summary>
    public delegate void CircuitWaitHandler(int value);

    public class DerbyGameplayCircuitBootstrap : MonoBehaviour {

        private static CircuitStartHandler CircuitStartCallback;
        private static CircuitWaitHandler CircuitWaitCallback;

        /// <summary>
        /// An event which invokes when the circuit begins.
        /// </summary>
        public static event CircuitStartHandler OnCircuitStart {
            add     { CircuitStartCallback += value; }
            remove  { CircuitStartCallback -= value; }
        }

        public static event CircuitWaitHandler OnCircuitWait {
            add     { CircuitWaitCallback += value; }
            remove  { CircuitWaitCallback -= value; }
        }

        [SerializeField, Tooltip("How many seconds should the circuit wait until it begins!")]
        private int delayTime = 3;
        [SerializeField]
        private int timeToWait = 1;

        private int time;

        private void Start() {
            time = delayTime;
            
            StartCoroutine(DelayTillCircuitStart());
        }

        private IEnumerator DelayTillCircuitStart() {
            while (time > 0) {
                yield return new WaitForSeconds(timeToWait);
                time -= timeToWait;
                if (CircuitWaitCallback != null) {
                    // TODO: Add the UI functionality.
                    CircuitWaitCallback(time);
                }
            }

            if (CircuitStartCallback != null) {
                CircuitStartCallback();
            }
        }
    }
}
