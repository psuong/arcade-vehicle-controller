using System.Collections.ObjectModel;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Derby {

    public sealed class VehicleEntityHelper : BaseEntity, IDictionaryWrapper<Collider, int> {

#if UNITY_EDITOR       
        /// <summary>
        /// Should the velocity of the spawned vehicle be drawn visually?
        /// </summary>
        public bool DebugVelocity { get; set; }
#endif

        private SelectionCache cache;
        private VehicleProfiles profile;

        // TODO: Make a set up method to avoid copying the code.
#if UNITY_EDITOR
        public VehicleEntityHelper(SelectionCache cache, VehicleProfiles profile, int size, bool debug) {
            DebugVelocity = debug;
            InternalSetUp(cache, profile, size);
        }
#endif

        public VehicleEntityHelper(SelectionCache cache, VehicleProfiles profile, int size) {
            InternalSetUp(cache, profile, size);
        }

        protected override void SetUp(int size) {
            for (int i = 0; i < size; i++) {
                var entity = entities[i];
                var gameObject = GameObject.Instantiate(profile[cache[i]]);

#if UNITY_EDITOR
                if (DebugVelocity)
                    TryAddVelocityDrawer(gameObject);
#endif

                entityManager.SetSharedComponentData(entity, new RigidbodyInstance { 
                        rigidbody = gameObject.GetComponent<Rigidbody>() 
                        });

                // Set up the player id
                entityManager.SetComponentData(entity, new IntId { value = i });

                // Set up the car movement constraint (dead zone)
                entityManager.SetComponentData(entity, new FloatId{
                        value = gameObject.GetComponent<VehicleMovementConstraintComponent>().Value.value
                        });

                var vehicleSpeed = gameObject.GetComponent<VehicleSpeedComponent>().Value;

                entityManager.SetComponentData(entity, new VehicleSpeed {
                        forwardSpeed = vehicleSpeed.forwardSpeed,
                        backwardSpeed = vehicleSpeed.backwardSpeed,
                        turnSpeed = vehicleSpeed.turnSpeed,
                        drag = vehicleSpeed.drag
                        });

                var hoverPoints = gameObject.GetComponent<HoverPointsComponent>().Value;

                entityManager.SetSharedComponentData(entity, new HoverPoints {
                        hoverPoints = hoverPoints.hoverPoints
                        });

                var vehiclePhysics = gameObject.GetComponent<VehiclePhysicsComponent>().Value;

                entityManager.SetComponentData(entity, new VehiclePhysics {
                        hover = vehiclePhysics.hover,
                        gravity = vehiclePhysics.gravity,
                        height = vehiclePhysics.height
                        });

                var playerCamera = gameObject.GetComponent<PlayerCameraComponent>().Value;

                entityManager.SetComponentData(entity, new PlayerCamera {
                        translationSpeed = playerCamera.translationSpeed,
                        rotationSpeed = playerCamera.rotationSpeed,
                        minDistance = playerCamera.minDistance,
                        lookAheadFactor = playerCamera.lookAheadFactor
                        });

                entityManager.SetSharedComponentData(entity, new ColliderInstances { values = gameObject.GetComponentsInChildren<Collider>() });

                // Set the rotational and positional data
                entityManager.SetComponentData(entity, new Position { Value = gameObject.transform.position });
                entityManager.SetComponentData(entity, new Rotation { Value = gameObject.transform.rotation });

                ECSUtils.RemoveComponentWrappers(gameObject, 
                    typeof(VehicleMovementConstraintComponent), typeof(VehicleSpeedComponent), 
                    typeof(HoverPointsComponent), typeof(VehiclePhysicsComponent), typeof(PlayerCameraComponent)
                );
            }
        }

        private void InternalSetUp(SelectionCache cache, VehicleProfiles profile, int size) {
            this.cache = cache;
            this.profile = profile;
            entities = new NativeArray<Entity>(size, Allocator.Persistent);
            entityManager = World.Active.GetExistingManager<EntityManager>();

            Archetype =  entityManager.CreateArchetype(
                    typeof(IntId), typeof(FloatId), typeof(VehicleMovement),
                    typeof(VehiclePhysics), typeof(RigidbodyInstance), typeof(PlayerInput),
                    typeof(VehicleSpeed), typeof(HoverPoints), typeof(Position), 
                    typeof(Rotation), typeof(TransformMatrix), typeof(PlayerCamera),
                    typeof(ColliderInstances)
                    );
            entityManager.CreateEntity(Archetype, entities);

            SetUp(size);
        }

        public void TryAddVelocityDrawer(GameObject gameObject) {
            if (!gameObject.GetComponent<Utils.VelocityDrawer>()) {
                gameObject.AddComponent<Utils.VelocityDrawer>();
            }
        }

        public ReadOnlyDictionary<Collider, int> GenerateMap() {
            var dict = new System.Collections.Generic.Dictionary<Collider, int>();
            foreach (var entity in entities) {
                var intId = entityManager.GetComponentData<IntId>(entity);
                var colliders = entityManager.GetSharedComponentData<ColliderInstances>(entity);

                foreach (var collider in colliders.values) {
                    dict.Add(collider, intId.value);
                }
            }

            return new ReadOnlyDictionary<Collider, int>(dict);
        }
    }
}
