using System;
using Unity.Entities;

namespace Derby {

    [Serializable]
    public struct IntId : IComponentData {
        public int value;

        public IntId(int i) {
            value = i;
        }
    }

    [Serializable]
    public struct FloatId : IComponentData {
        public float value;

        public FloatId(float i) {
            value = i;
        }
    }

    public struct IntIdComponent {
        public ComponentDataArray<IntId> ids;
        public readonly int Length;
    }
}
