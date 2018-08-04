using UnityEngine;

namespace Derby {

    public static class CameraUtils {

        /// <summary>
        /// Sets up the camera's viewport depending on the index relative to the max size.
        /// </summary>
        /// <param name="camera">A reference to the actual camera.</param>
        /// <param name="i">The index of the camera, inclusive of the lhs and rhs.</param>
        /// <param name="max">The total number of cameras.</param>
        public static void SetUpCamera(ref Camera camera, int i, int max) {
            if (max == 1) 
                return;

            i += 1;

            camera.rect = new Rect {
                x = i % 2 == 0 ? 0.5f : 0,
                y = i <= 2 ? 0 : 0.5f,
                width = 0.5f,
                height = max<= 2 ? 1f : 0.5f
            };
        }
    }

}