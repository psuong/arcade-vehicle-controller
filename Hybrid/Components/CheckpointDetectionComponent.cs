using System;
using Unity.Entities;
using UnityEngine;

namespace Derby {

    [Serializable]
    public struct PairPoints {
        public Vector3 lhs, rhs;
    }

    [Serializable]
    public struct CheckpointDetection : ISharedComponentData {
        // TODO: Remove this as it's useless data
        public string name;
        public Matrix4x4 matrix;
        public PairPoints[] values;
        public readonly int Length;
    }

    public class CheckpointDetectionComponent : SharedComponentDataWrapper<CheckpointDetection> { }
}
