using SadConsoleTemplate.Entities.Core;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SadConsoleTemplate.Managers.Entities
{
    /// <summary>
    /// A synchronized implementation for managing entities.
    /// </summary>
    internal class SynchronizedEntityManager : EntityManager
    {
        private readonly Dictionary<Point, HashSet<Actor>> _entities = new();

        /// <inheritdoc/>
        public override void SpawnAt(Point position, Actor actor)
        {
            if (!_entities.TryGetValue(position, out var entities))
            {
                _entities.Add(position, entities = new HashSet<Actor>());
            }
            entities.Add(actor);
            actor.Position = position;
            actor.PositionChanged += UpdateEntityPositionWithinManager;
            EntityComponent.Add(actor);
        }

        /// <inheritdoc/>
        public override T CreateAt<T>(Point position)
        {
            var actor = new T();
            SpawnAt(position, actor);
            return actor;
        }

        /// <inheritdoc/>
        public override IEnumerable<Actor> RemoveAll(Point position, Func<Actor, bool> criteria = null)
        {
            if (_entities.TryGetValue(position, out HashSet<Actor> actors))
            {
                IEnumerable<Actor> actorsToBeRemoved = actors;
                if (criteria != null)
                    actorsToBeRemoved = actorsToBeRemoved.Where(criteria);
                foreach (var actor in actorsToBeRemoved.ToArray())
                {
                    EntityComponent.Remove(actor);
                    actor.PositionChanged -= UpdateEntityPositionWithinManager;
                    actors.Remove(actor);
                    yield return actor;
                }

                if (actors.Count == 0)
                    _entities.Remove(position);
            }
        }

        /// <inheritdoc/>
        public override void Remove(Actor actor)
        {
            if (_entities.TryGetValue(actor.Position, out HashSet<Actor> actors))
            {
                actor.PositionChanged -= UpdateEntityPositionWithinManager;
                actors.Remove(actor);
            }
        }

        /// <inheritdoc/>
        public override bool EntitiesExistAt(Point point)
        {
            return _entities.ContainsKey(point);
        }

        /// <inheritdoc/>
        public override IEnumerable<Actor> GetEntitiesAt(Point point)
        {
            return _entities.TryGetValue(point, out HashSet<Actor> actors) ? actors : Enumerable.Empty<Actor>();
        }

        /// <inheritdoc/>
        protected override void UpdateEntityPositionWithinManager(object sender, ValueChangedEventArgs<Point> e)
        {
            var actor = (Actor)sender;

            // Remove from previous
            if (_entities.TryGetValue(e.OldValue, out HashSet<Actor> actors))
            {
                actors.Remove(actor);
                if (actors.Count == 0)
                    _entities.Remove(e.OldValue);
            }

            // Add to the current
            if (!_entities.TryGetValue(e.NewValue, out actors))
            {
                _entities.Add(e.NewValue, actors = new HashSet<Actor>());
            }
            actors.Add(actor);
        }
    }
}
