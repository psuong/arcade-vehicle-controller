using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Derby {

    public sealed class CheckpointEntityHelper : BaseEntity, IDictionaryWrapper<Collider, int> {
        
        /// <summary>
        /// Generates all of the entities to be managed by the entity manager.
        /// </summary>
        /// <param name="size">How many entities do we instantiate?</param>
        /// <param name="template">Is there a prototype of the entity that can be used as a base?</param>
        /// <param name="transforms">What are the transforms to use when initializing?</param>
        public CheckpointEntityHelper(int size, GameObject template, Transform[] transforms) {
            entityManager = World.Active.GetExistingManager<EntityManager>();
            entities = new NativeArray<Entity>(size, Allocator.Persistent);

            Archetype = entityManager.CreateArchetype(
                typeof(MeshInstanceRenderer), typeof(Position), typeof(Rotation), typeof(Scale),
                typeof(TransformMatrix), typeof(CheckpointDetection), typeof(ColliderInstances),
                typeof(IntId)
            );

            entityManager.CreateEntity(Archetype, entities);
            InstantiateEntity(size, template, transforms);
        }

        ~CheckpointEntityHelper() {
            entities.Dispose();
        }

        private void InstantiateEntity(int size, GameObject template, Transform[] transforms) {
            for (int i = 0; i < size; i++) {
                var origName = transforms[i].name;
                var position = transforms[i].position;
                var rotation = transforms[i].rotation;
                
                var gameObject = GameObject.Instantiate(template, position, rotation);
                gameObject.name = origName;
                var entity = entities[i];

                // Set the index value of the checkpoint
                entityManager.SetComponentData(entity, new IntId { value = i });

                entityManager.SetComponentData(entity, new Position { Value = position });
                entityManager.SetComponentData(entity, new Rotation { Value = rotation });
                entityManager.SetComponentData(entity, new Scale { Value = template.GetComponent<ScaleComponent>().Value.Value });

                var meshInstance = gameObject.GetComponent<MeshInstanceRendererComponent>().Value;

                entityManager.SetSharedComponentData(entity , new MeshInstanceRenderer {
                    mesh = meshInstance.mesh,
                    material = meshInstance.material,
                    receiveShadows = true,
                    castShadows = UnityEngine.Rendering.ShadowCastingMode.On
                });

                var checkpoint = gameObject.GetComponent<CheckpointDetectionComponent>().Value;

                entityManager.SetSharedComponentData(entity, new CheckpointDetection { 
                    name = origName,
                    matrix = gameObject.transform.localToWorldMatrix,
                    values = checkpoint.values 
                });

                var colliders = gameObject.GetComponentsInChildren<Collider>();

                entityManager.SetSharedComponentData(entity, new ColliderInstances {
                    values = colliders
                });

                ECSUtils.RemoveComponentWrappers(gameObject, 
                    typeof(MeshInstanceRenderer), typeof(ScaleComponent), typeof(CheckpointDetectionComponent), 
                    typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshInstanceRendererComponent)
                );
            }
        }

        public ReadOnlyDictionary<Collider, int> GenerateMap() {
            var dict = new Dictionary<Collider, int>();
            foreach (var entity in entities) {
                var colliders = entityManager.GetSharedComponentData<ColliderInstances>(entity);
                var id = entityManager.GetComponentData<IntId>(entity).value;

                foreach (var collider in colliders.values) {
                    dict.Add(collider, id);
                }
            }

            return new ReadOnlyDictionary<Collider, int>(dict);
        }
    }
}
