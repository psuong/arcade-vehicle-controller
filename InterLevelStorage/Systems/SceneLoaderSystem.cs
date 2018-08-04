using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Derby.SceneManagement {
    
    /// <summary>
    /// Adds a delegate to invoke the next scene to load based on an index.
    /// </summary>
    public delegate void SceneIndexHandler(int loadIndex, int unloadIndex, LoadSceneMode mode);

    /// <summary>
    /// What should happen when the scene is loading?
    /// </summary>
    public delegate void SceneLoadStartHandler();

    /// <summary>
    /// What should happen when the scene is unloading?
    /// </summary>
    public delegate void SceneUnloadStartHandler();

    /// <summary>
    /// What should happen when the scene is finished loading?
    /// </summary>
    public delegate void SceneLoadEndHandler();
    
    /// <summary>
    /// Adds a delegate to invoke a series of scene indices.
    /// </summary>
    public delegate void SceneIndicesHandler(LoadSceneMode mode, params int[] indices);

    public class SceneLoaderSystem : MonoBehaviour {

        private static SceneIndexHandler sceneIndexHandler;
        private static SceneIndicesHandler sceneIndiciesHandler;
        private static SceneLoadStartHandler sceneLoadStartHandler;
        private static SceneLoadEndHandler sceneLoadEndHandler;
        
        /// <summary>
        /// which scene should load and how should it load?
        /// </summary>
        public static event SceneIndexHandler OnSceneLoadRequest {
            add     { sceneIndexHandler += value; }
            remove  { sceneIndexHandler -= value; }
        }

        /// <summary>
        /// Which scenes should load and how should they load?
        /// </summary>
        public static event SceneIndicesHandler OnScenesLoad {
            add     { sceneIndiciesHandler += value; }
            remove  { sceneIndiciesHandler -= value; }
        }
        
        /// <summary>
        /// What event should happen when the scene immediately begins loading?
        /// </summary>
        public static event SceneLoadStartHandler OnSceneLoadStart {
            add     { sceneLoadStartHandler += value; }
            remove  { sceneLoadStartHandler -= value; }
        }
        
        /// <summary>    
        /// What event should happen when the scene finishes loading? 
        /// </summary>
        public static event SceneLoadEndHandler OnSceneLoadEnd {
            add     { sceneLoadEndHandler += value; }
            remove  { sceneLoadEndHandler -= value; }
        }
        /// <summary>
        /// Safely invokes the delegate to load and unload scenes.
        /// </summary>
        /// <param name="loadIndex">Which scene should load?</param>
        /// <param name="unloadIndex">Which scene should unload?</param>
        /// <param name="mode">How should the scene(s) unload?</param>
        public static void OnSceneLoadRequestSafeInvoke(int loadIndex, int unloadIndex, LoadSceneMode mode) {
            if (sceneIndexHandler != null) {
                sceneIndexHandler(loadIndex, unloadIndex, mode);
            }
#if UNITY_EDITOR
            else {
                Debug.LogError("No function was registered to OnSceneLoadRequest!");
            }
#endif
        }
        
        [SerializeField]
        private ProgressCache cache;

        private void OnEnable() {
            sceneIndexHandler += LoadSceneAndUnloadSceneIndex;
        }

        private void OnDisable() {
            sceneIndexHandler -= LoadSceneAndUnloadSceneIndex;
        }

        private void LoadSceneAndUnloadSceneIndex(int loadIndex, int unloadIndex, LoadSceneMode mode) {
#if UNITY_EDITOR
            Debug.LogFormat("Loading: {0}: {1}, Unloading: {2}: {3}", loadIndex, SceneManager.GetSceneByBuildIndex(loadIndex).name, unloadIndex, SceneManager.GetSceneByBuildIndex(unloadIndex).name);
#endif
            StartCoroutine(YieldUntilSceneLoaded(loadIndex, mode));
            StartCoroutine(YieldUntilSceneUnloaded(unloadIndex));
            // Dispose the cache
            cache.Reset();
        }

        private IEnumerator YieldUntilSceneLoaded(int loadIndex, LoadSceneMode mode) {
            var asyncStatus = SceneManager.LoadSceneAsync(loadIndex, mode);
            asyncStatus.allowSceneActivation = true;

            if (sceneLoadStartHandler != null && asyncStatus != null) {
                sceneLoadStartHandler();
            }

            while (!asyncStatus.isDone) {
                cache.UpdateProgress(asyncStatus.progress);
                yield return null;
            }

            cache.Reset();
            
            // Get the scene loaded recently and make it active.
            var scene = SceneManager.GetSceneByBuildIndex(loadIndex);
#if UNITY_EDITOR
            Debug.LogFormat("<color=#add8e6ff>Setting {0} as the active scene.</color>", scene.name);
#endif
            SceneManager.SetActiveScene(scene);

            // Invoke the finished handler when we're done loading the scene.
            if (sceneLoadEndHandler != null) {
                sceneLoadEndHandler();
            }
        }

        private IEnumerator YieldUntilSceneUnloaded(int unloadIndex) {
            var asyncStatus = SceneManager.UnloadSceneAsync(unloadIndex);
            asyncStatus.allowSceneActivation = true;

            while (!asyncStatus.isDone) {
                yield return null;
            }
        }
    }
}
