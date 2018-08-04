using Derby.Systems;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Assertions;

namespace Derby {

    public sealed class VehicleSelectionScreenBootStrap : MonoBehaviour {

        public static PlayerPool PlayerPool {
            get;
            private set;
        }

        public static SelectionCache VehicleSelectionCache {
            get;
            private set;
        }

        [SerializeField]
        private Camera template;
        [SerializeField]
        private PlayerPool playerPool;
        [SerializeField]
        private SelectionCache cache;

        private EntityArchetype browseArchetype;
        private EntityManager entityManager;
        private NativeArray<Entity> browseEntities; 


        private void Awake() {
            Assert.IsNotNull(template, "No Camera found!");
            Assert.IsNotNull(playerPool, "No PlayerPool found!");
            Assert.IsNotNull(cache, "No SelectionCache found!");
            
            PlayerPool = playerPool;
            VehicleSelectionCache = cache;

            // TODO: Uncomment
            ECSUtils.DisableSystems(typeof(GameplayPlayerPhysicsMoveSystem), typeof(GameplayPlayerHoverSystem));

#if UNITY_EDITOR
            ECSUtils.DisableSystems(typeof(Derby.Tests.CameraTestSystem), typeof(Derby.Tests.CameraTransformUpdater));
#endif
        }

        private void OnDestroy() {
            browseEntities.Dispose();

            ECSUtils.DisableSystems(typeof(VehicleSelectionScreenQueueSystem), typeof(VehicleSelectionScreenInputHandler), 
                typeof(VehicleSelectionScreenCameraSystem), typeof(VehicleSelectionScreenCameraTransformSystem));
        }

        private void Start() {
            entityManager = World.Active.GetOrCreateManager<EntityManager>();

            // Initialize inter scene storage...
            playerPool.Initialize();
            cache.Initialize(playerPool.ActivePlayerCount);

            SetUpArchetypes();
            StartCoroutine(SetUpCamera());
        }

        private void SetUpArchetypes() {
            // Create archetypes
            browseArchetype = entityManager.CreateArchetype(
               typeof(SelectionBrowseData), typeof(CameraInstance), typeof(Position),
               typeof(TransformMatrix), typeof(IntId), typeof(CameraTracker), typeof(SelectionConfirmationData));
        }

        private IEnumerator SetUpCamera() {
            yield return new WaitForEndOfFrame();

            var start = template.transform.position;

            var size = playerPool.ActivePlayerCount;
            browseEntities= new NativeArray<Entity>(size, Allocator.Persistent);

            entityManager.CreateEntity(browseArchetype, browseEntities);

            // TODO: Align the cameras properly based on the player id.
            for (int i = 0; i < size; i++) {
                SetUpCameraEntity(i);
            }

            Destroy(template.gameObject);
        }

        private void SetUpCameraEntity(int i) {
            var entity = browseEntities[i];
            var camera = Instantiate(template).GetComponent<Camera>();

            entityManager.SetComponentData(entity, new Position(template.transform.position));
            entityManager.SetComponentData(entity, new IntId(i));

            entityManager.SetSharedComponentData(entity, new CameraInstance {
                camera = camera
            });

            CameraUtils.SetUpCamera(ref camera, i, playerPool.ActivePlayerCount);
        }
    }
}
