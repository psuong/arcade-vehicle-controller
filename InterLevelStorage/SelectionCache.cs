using UnityEngine;

namespace Derby {

    /// <summary>
    /// A general purpose cache to store index based data and to pass around between scenes / systems / objects.
    /// </summary>
    [CreateAssetMenu(menuName = "Derby/Selection Cache", fileName = "Selection Cache")]
    public class SelectionCache : ScriptableObject, IArrayWrapper<int> {

        public int Length {
            get {
                return selections.Length;
            }
        }

        public int this[int i] {
            get {
                return selections[i];
            }
            set {
                selections[i] = value;
            }
        }

        // Store the indices of the car selections
        // Each index is the player number.
        public int[] selections;

        public void Initialize(int size) {
            selections = new int[size];

            for (int i = 0; i < size; i++) {
                selections[i] = -1;
            }
        }

        public void Dispose() {
            for (int i = 0; i < selections.Length; i++) {
                selections[i] = -1;
            }
        }
    }
}
