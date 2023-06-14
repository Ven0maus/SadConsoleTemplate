using SadConsole.Entities;
using SadConsoleTemplate.Entities.Core;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;

namespace SadConsoleTemplate.Managers.Entities
{
    internal abstract class EntityManager
    {
        /// <summary>
        /// Renderer component that the entities are using to render, hook this to a console.
        /// </summary>
        public readonly Renderer EntityComponent = new();
        /// <summary>
        /// Inserts the actor at the given location within the entity manager.
        /// This method is meant to insert an entity, after it was created.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="actor"></param>
        public abstract void SpawnAt(Point position, Actor actor);
        /// <summary>
        /// Creates a new entity of the given type at the specified location within the entity manager.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <returns></returns>
        public abstract T CreateAt<T>(Point position) where T : Actor, new();
        /// <summary>
        /// Removes all the entities from the given position, based on optional criteria.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="criteria"></param>
        /// <returns>Removed entities</returns>
        public abstract IEnumerable<Actor> RemoveAll(Point position, Func<Actor, bool> criteria = null);
        /// <summary>
        /// Removes the actor from the given position.
        /// </summary>
        /// <param name="actor"></param>
        public abstract void Remove(Actor actor);
        /// <summary>
        /// Returns true if entities exist at the given location, false if not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>True/False</returns>
        public abstract bool EntitiesExistAt(Point point);
        /// <summary>
        /// Returns all the entities at the given position.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Entities at point</returns>
        public abstract IEnumerable<Actor> GetEntitiesAt(Point point);
        /// <summary>
        /// Keeps the location of each entity synced in the manager as they move around.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="previous"></param>
        /// <param name="current"></param>
        protected abstract void UpdateEntityPositionWithinManager(object sender, ValueChangedEventArgs<Point> e);
    }
}
