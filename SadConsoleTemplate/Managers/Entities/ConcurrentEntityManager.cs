using SadConsoleTemplate.Entities.Core;
using SadRogue.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SadConsoleTemplate.Managers.Entities
{
    /// <summary>
    /// A concurrent implementation for managing entities.
    /// </summary>
    internal class ConcurrentEntityManager : EntityManager
    {
        private readonly ConcurrentDictionary<Point, HashSet<Actor>> _entities = new();
        private readonly ConcurrentDictionary<Point, object> _lockObjects = new();

        /// <summary>
        /// Helper method to lock the hashset collection within.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="action"></param>
        private void ApplyActionOnHashSet(Point point, Action<HashSet<Actor>> action)
        {
            // Get or create the hash set for a specific Point
            HashSet<Actor> actors = _entities.GetOrAdd(point, _ => new HashSet<Actor>());

            // Acquire the lock object associated with the hash set
            object lockObj = _lockObjects.GetOrAdd(point, _ => new object());

            // Enter the lock
            Monitor.Enter(lockObj);
            try
            {
                action(actors);
            }
            finally
            {
                // Ensure the lock is released
                Monitor.Exit(lockObj);

                // Remove the lock object from the dictionary
                _lockObjects.TryRemove(point, out _);
            }
        }

        /// <inheritdoc/>
        public override void SpawnAt(Point position, Actor actor)
        {
            ApplyActionOnHashSet(position, (actors) => 
            { 
                actors.Add(actor);
                actor.Position = position;
                actor.PositionChanged += UpdateEntityPositionWithinManager;
                EntityComponent.Add(actor);
            });
        }

        /// <inheritdoc/>
        public override T CreateAt<T>(Point position)
        {
            var actor = new T();
            SpawnAt(position, actor);
            return actor;
        }

        /// <inheritdoc/>
        public override bool EntitiesExistAt(Point point)
        {
            return _entities.ContainsKey(point);
        }

        /// <inheritdoc/>
        public override IEnumerable<Actor> GetEntitiesAt(Point point)
        {
            IEnumerable<Actor> actorsEnumerable = Enumerable.Empty<Actor>();
            if (!EntitiesExistAt(point)) return actorsEnumerable;
            ApplyActionOnHashSet(point, (actors) =>
            {
                actorsEnumerable = actors.ToArray();
            });
            return actorsEnumerable;
        }

        /// <inheritdoc/>
        public override void Remove(Actor actor)
        {
            if (!EntitiesExistAt(actor.Position)) return;
            ApplyActionOnHashSet(actor.Position, (actors) =>
            {
                actor.PositionChanged -= UpdateEntityPositionWithinManager;
                actors.Remove(actor);
            });
        }

        /// <inheritdoc/>
        public override IEnumerable<Actor> RemoveAll(Point position, Func<Actor, bool> criteria = null)
        {
            if (!EntitiesExistAt(position)) return Enumerable.Empty<Actor>();
            var removedActors = new List<Actor>();
            ApplyActionOnHashSet(position, (actors) =>
            {
                IEnumerable<Actor> actorsToBeRemoved = actors;
                if (criteria != null)
                    actorsToBeRemoved = actorsToBeRemoved.Where(criteria);
                foreach (var actor in actorsToBeRemoved.ToArray())
                {
                    EntityComponent.Remove(actor);
                    actor.PositionChanged -= UpdateEntityPositionWithinManager;
                    actors.Remove(actor);
                    removedActors.Add(actor);
                }

                if (actors.Count == 0)
                    _entities.Remove(position, out _);
            });
            return removedActors;
        }

        /// <inheritdoc/>
        protected override void UpdateEntityPositionWithinManager(object sender, ValueChangedEventArgs<Point> e)
        {
            var actor = (Actor)sender;

            // Remove from previous
            if (EntitiesExistAt(e.OldValue))
            {
                ApplyActionOnHashSet(e.OldValue, (actors) =>
                {
                    actors.Remove(actor);
                    if (actors.Count == 0)
                        _entities.Remove(e.OldValue, out _);
                });
            }

            // Add to the current
            ApplyActionOnHashSet(e.NewValue, (actors) =>
            {
                actors.Add(actor);
            });
        }
    }
}
