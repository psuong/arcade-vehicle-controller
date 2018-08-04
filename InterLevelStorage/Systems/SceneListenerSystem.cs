using UnityEngine;
using UnityEngine.SceneManagement;

namespace Derby.SceneManagement {
    
    /// <summary>
    /// Handles write data of levels selected and invokes an event to the scene loading system.
    /// </summary>
    // TODO: Handle multiple scenes which need to load.
    public class SceneListenerSystem : MonoBehaviour {

        public static SelectionCache LevelCache { get; private set; }

        [SerializeField, Tooltip("What levels did the player(s) pick?")]
        private SelectionCache cache;
        [SerializeField]
        private PlayerPool pool;
        [SerializeField]
        private LoadSceneMode mode;
        [SerializeField, Tooltip("What scene should be unloaded from the build settings?")]
        private int unloadSceneIndex;
        [SerializeField]
        private bool isLevelPredetermined;
        [SerializeField]
        private int nextLevelToLoad;

        private bool isInvoked;

        private void Awake() {
            isInvoked = false;

#if UNITY_EDITOR
            Debug.LogFormat("<color=#ff00ffff>The scene you're trying to unload is: {0}</color>", SceneManager.GetSceneByBuildIndex(unloadSceneIndex).name);
#endif
        }

        private void Start() {
            // cache.Initialize(pool.ActivePlayerCount);
            // TODO: Remove this cause it's a test
            cache.Initialize(1);
        }

        private void Update() {
            if (cache.selections.AreElementsEquivalent() && AreSelectionsValid() && !isInvoked) {

#if UNITY_EDITOR
                if (isLevelPredetermined) {
                    Debug.LogFormat("<color=#ff00ffff>The predetermined scene you're trying to load is: {0}</color>", SceneManager.GetSceneByBuildIndex(nextLevelToLoad).name);               
                }
#endif
                SceneLoaderSystem.OnSceneLoadRequestSafeInvoke(isLevelPredetermined ? nextLevelToLoad : cache[0], unloadSceneIndex, mode);
                isInvoked = true;
            }
        }

        // TODO: Preferrably I should make this a O(logn) operation
        private bool AreSelectionsValid() {
            for (int i = 0; i < cache.Length; i++) {
                if (cache[i] < 0) {
                    return false;
                }
            }
            return true;
        }
    }
}
