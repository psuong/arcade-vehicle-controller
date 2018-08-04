using Unity.Collections;
using Unity.Entities;

namespace Derby {
	public abstract class BaseEntity {
        
        public int EntitiesLength {
            get {
                return entities.Length;
            }
        }

		/// <summary>
		/// Convenience archetype.
		/// </summary>
		public EntityArchetype Archetype { get; protected set; }

		/// <summary>
		/// Stores all of the entities.
		/// </summary>
		protected NativeArray<Entity> entities;
        
        /// <summary>
        /// A reference to the entity manager.
        /// </summary>
        protected EntityManager entityManager;

        /// <summary>
        /// Sets up an entities component based solely on the index.
        /// </summary>
        /// <param name="size">How many entities do we need to set up?</param>
        protected virtual void SetUp(int size) { }

		/// <summary>
		/// Sets up the entity's component data.
		/// </summary>
		/// <param name="gameObject">The template to copy from.</param>
        /// <param name="size">How many entities do we need to set up?</param>
		protected virtual void SetUp(UnityEngine.GameObject gameObject, int size) { }

	}
}
