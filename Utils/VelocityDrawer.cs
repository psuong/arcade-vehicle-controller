using UnityEngine;

namespace Derby.Utils {

    public class VelocityDrawer : MonoBehaviour {
        
        [SerializeField]
        private Color positiveColour = Color.green;
        [SerializeField]
        private Color negativeColour = Color.red;
        [SerializeField]
        private float radius = 0.5f;
        [SerializeField]
        private bool drawForward;

        private Rigidbody rbody;

        private void Start() {
            rbody = GetComponent<Rigidbody>();
        }
        
        private void OnDrawGizmos() {
            var velocity = rbody.velocity;

            if (drawForward) {
                Gizmos.color = positiveColour;
                Gizmos.DrawSphere(velocity + transform.position, radius);
                Gizmos.DrawRay(transform.position, velocity);
            } else {
                Gizmos.color = negativeColour;
                Gizmos.DrawSphere(-velocity + transform.position, radius);
                Gizmos.DrawRay(transform.position, -velocity);
            }
        }
    }
}
