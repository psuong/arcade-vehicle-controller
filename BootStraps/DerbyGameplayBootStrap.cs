using Derby.Systems;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace Derby {

    public sealed class DerbyGameplayBootStrap : MonoBehaviour {

        public static PlayerPool PlayerPool {
            get; 
            private set;
        }

        public static SelectionCache VehicleSelectionCache {
            get;
            private set;
        }

        public static ReadOnlyDictionary<Collider, int> PlayerColliderMap {
            get;
            private set;
        }

        [SerializeField]
        private Camera cameraTemplate;
        [SerializeField]
        private GameObject checkpointTemplate;
        [SerializeField]
        private PlayerPool pool;
        [SerializeField]
        private SelectionCache cache;
        [SerializeField]
        private VehicleProfiles profile;
        [SerializeField]
        private PositionSelection spawnPoints;
#if UNITY_EDITOR
        [SerializeField]
        private bool debugVelocity;
#endif

        private CameraEntityHelper cameraHelper;
        private VehicleEntityHelper vehicleHelper; 

        private void Awake() {
            Assert.IsNotNull(cameraTemplate, "No Camera found!");
            Assert.IsNotNull(pool, "No PlayerPool found!");
            Assert.IsNotNull(cache, "No SelectionCache found!");
            Assert.IsTrue(spawnPoints.Length >= pool.ActivePlayerCount, "The number of spawn points is less than the number of active players!");

            PlayerPool = pool;
            VehicleSelectionCache = cache;
        }

        private void OnEnable() {
        }

        private void OnDisable() {
        }
        
        private void Start() {
            var size = cache.Length;
            SetUp();
#if UNITY_EDITOR
            vehicleHelper = new VehicleEntityHelper(cache, profile, size, debugVelocity);
            vehicleHelper.DebugVelocity = debugVelocity;
#else
            vehicleHelper = new VehicleEntityHelper(cache, profile, size);
#endif
            cameraHelper = new CameraEntityHelper(cameraTemplate.gameObject, size);

            PlayerColliderMap = vehicleHelper.GenerateMap();
        }

        private void SetUp() {
            ECSUtils.EnableSystems(typeof(GameplayPlayerHoverSystem), typeof(GameplayPlayerPhysicsMoveSystem), typeof(GameplayInputHandler), 
                typeof(GameplayPlayerInputInterpreter));
        }
    }
}
