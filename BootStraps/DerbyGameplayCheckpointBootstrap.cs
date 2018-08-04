using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace Derby {

    public sealed class DerbyGameplayCheckpointBootstrap : MonoBehaviour {
        
        /// <summary>
        /// Stores a universal read / write array encapsulated in a scriptable object.  
        /// </summary>
        public static LapTracker LapTracker { 
            get; 
            private set;
        }
            
        /// <summary>
        /// Stores all of the entities associated with their colliders.
        /// </summary>
        public static ReadOnlyDictionary<Collider, int> CheckpointColliderMap {
            get;
            private set;
        }

        [SerializeField]
        private GameObject template;
        [SerializeField]
        private Transform[] spawnPoints;
        [SerializeField]
        private LapTracker lapTracker;
        [SerializeField]
        private PlayerPool pool;

        private CheckpointEntityHelper checkpointHelper;

        private void Awake() {
            Assert.IsNotNull(lapTracker, "No LapTracker found!");
            Assert.IsNotNull(lapTracker, "No PlayerPool found!");
            
            LapTracker = lapTracker;
        }

        private void Start() {
            checkpointHelper = new CheckpointEntityHelper(spawnPoints.Length, template, spawnPoints);
            CheckpointColliderMap = checkpointHelper.GenerateMap();
            StartCoroutine(SetUp());
        }

        // TODO: Check if this is consistent.
        private IEnumerator SetUp() {
            yield return new WaitForEndOfFrame();
            lapTracker.Initialize(pool.ActivePlayerCount, checkpointHelper.EntitiesLength);
        }
    }
}
