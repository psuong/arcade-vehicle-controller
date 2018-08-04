using System;
using Unity.Entities;
using UnityEngine;

namespace Derby {
	[Serializable]
	public struct CameraInstance : ISharedComponentData {
		public Camera camera;
	}

	public class CameraInstanceComponent : SharedComponentDataWrapper<CameraInstance> { }
}
