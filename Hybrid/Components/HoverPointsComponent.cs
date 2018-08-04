using System;
using Unity.Entities;
using UnityEngine;

namespace Derby {

    /// <summary>
    /// Store an array of float3 that each vehicle components has.
    /// </summary>
    [Serializable]
    public struct HoverPoints : ISharedComponentData {
        public Transform[] hoverPoints;
    }

    public class HoverPointsComponent : SharedComponentDataWrapper<HoverPoints> { }
}
