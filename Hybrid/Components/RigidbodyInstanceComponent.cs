using System;
using Unity.Entities;

namespace Derby {

	[Serializable]
	public struct RigidbodyInstance : ISharedComponentData {
		public UnityEngine.Rigidbody rigidbody;
	}

	public class RigidbodyInstanceComponent : SharedComponentDataWrapper<RigidbodyInstance> { }
}