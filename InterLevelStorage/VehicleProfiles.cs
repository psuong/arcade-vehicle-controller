using System.Collections.Generic;
using UnityEngine;

namespace Derby {

    /**
     * Stores all of the Car Profiles to instantiate.
     */
    [CreateAssetMenu(menuName = "Derby/Vehicle Profile", fileName = "Vehicle Profile")]
    public class VehicleProfiles : ScriptableObject, IArrayWrapper<GameObject> {

        public IList<GameObject> Cars {
            get {
                return System.Array.AsReadOnly(cars);
            }
        }

        public int Length {
            get {
                return cars.Length;
            }
        }

        public GameObject this[int i] {
            get {
                return cars[i];
            }
            set {
                cars[i] = value;
            }
        }

        [SerializeField]
        private GameObject[] cars;
    }
}
