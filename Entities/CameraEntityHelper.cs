using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Derby {

    public sealed class CameraEntityHelper : BaseEntity {

        public CameraEntityHelper(GameObject gameObject, int size) {
            entities = new NativeArray<Entity>(size, Allocator.Persistent);
            entityManager = World.Active.GetExistingManager<EntityManager>();

            Archetype = entityManager.CreateArchetype(typeof(IntId), typeof(Position), typeof(Rotation),
                typeof(TransformMatrix), typeof(CameraInstance));

            entityManager.CreateEntity(Archetype, entities);

            SetUp(gameObject, size);
        }

        ~CameraEntityHelper() {
            entities.Dispose();
        }

        protected override void SetUp(GameObject gameObject, int size) {
            for (int i = 0; i < entities.Length; i++) {
                var template = GameObject.Instantiate(gameObject);
                var camera = template.GetComponent<Camera>();
                var entity = entities[i];

                entityManager.SetComponentData(entity, new IntId { value = i });
                entityManager.SetSharedComponentData(entity, new CameraInstance {
                    camera = template.GetComponent<Camera>()
                });

                CameraUtils.SetUpCamera(ref camera, i, size);

                ECSUtils.RemoveComponentWrappers(template, typeof(CameraInstanceComponent));
            }
        }
    }
}
