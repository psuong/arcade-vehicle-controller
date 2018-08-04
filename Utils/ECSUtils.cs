using System;
using Unity.Entities;
using UnityEngine;

namespace Derby {

    public static class ECSUtils {

        /// <summary>
        /// Disables a set of systems within the EntityManager.
        /// </summary>
        /// <param name="types">A series of types that must derive from ComponentSystem to disable.</param>
        public static void DisableSystems(params System.Type[] types) {
            var world = World.Active;
            foreach (var type in types) {
                TrySetSystemState(type, world, false);
            }
        }

        /// <summary>
        /// Destroys all registered systems within the ECS ecosystem.
        /// </summary>
        /// <param name="types">A series of types that must dervice from ComponentSystem.</param>
        public static void DestroySystems(params System.Type[] types) {
            var world = World.Active;

            foreach (var type in types) {
                var system = ((ComponentSystem)world.GetExistingManager(type));
                world.DestroyManager(system);
            }
        }

        /// <summary>
        /// Enables a set of systems within the EntityManager.
        /// </summary>
        /// <param name="types">A series of types that must derive from ComponentSystem to disable.</param>
        public static void EnableSystems(params System.Type[] types) {
            var world = World.Active;
            foreach (var type in types) {
                TrySetSystemState(type, world, true);
            }
        }

        /// <summary>
        /// Removes all ECS Component wrappers from the gameObject and its children. The GameObjectEntity is removed as the last step.
        /// </summary>
        /// <param name="gameObject">The gameObject to remove the components from.</param>
        /// <param name="types">The ComponentWrappers to remove.</param>
        public static void RemoveComponentWrappers(GameObject gameObject, params Type[] types) {
            foreach (var type in types) {
                if (type == (typeof(GameObjectEntity))) {
                    continue;
                }

                if (type.IsSubclassOf(typeof(ComponentDataWrapperBase))) { 
                    GameObject.Destroy(gameObject.GetComponentInChildren(type));
                }
            }
            GameObject.Destroy(gameObject.GetComponentInChildren<GameObjectEntity>());
        }


        private static void TrySetSystemState(System.Type type, World world, bool state) {
            var system = world.GetExistingManager(type) as ComponentSystemBase;
            if (system != null) {
                system.Enabled = state;
            }
#if UNITY_EDITOR
            else {
                UnityEngine.Debug.LogErrorFormat("Type: {0} does not derive from <ComponentSystem>!", type);
            }
#endif
        }
    }
}
