using Unity.Entities;
using UnityEngine;

namespace Derby {
    
    [System.Serializable]
    public struct ColliderInstances : ISharedComponentData {
        public Collider[] values;
        public readonly int Length;
    }

    public class ColliderInstancesComponent : SharedComponentDataWrapper<ColliderInstances> { }
}
