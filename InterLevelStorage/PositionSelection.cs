using UnityEngine;

namespace Derby {

    [CreateAssetMenu(menuName = "Derby/Positions", fileName = "Positions")]
    public class PositionSelection : ScriptableObject, IArrayWrapper<Vector3> {

        public int Length {
            get {
                return points.Length;
            }
        }

        public Vector3 this[int i] {
            get {
                return points[i];
            }
            set {
                points[i] = value;
            }
        }
        
        public Vector3[] points;
    }
}
