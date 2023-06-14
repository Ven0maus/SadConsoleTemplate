using SadConsole.Entities;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;

namespace SadConsoleTemplate.Entities
{
    internal static class EntityManager
    {
        public static readonly Renderer EntityComponent = new();
        private static readonly Dictionary<Point, Actor> _entities = new();

        public static void SpawnAt(Point position, Actor actor)
        {
            if (_entities.ContainsKey(position))
                throw new Exception("An entity already exists at this location.");
            actor.Position = position;
            EntityComponent.Add(actor);
            _entities.Add(position, actor);
        }

        public static T SpawnAt<T>(Point position) where T : Actor, new()
        {
            if (_entities.ContainsKey(position))
                throw new Exception("An entity already exists at this location.");
            var actor = new T
            {
                Position = position
            };
            EntityComponent.Add(actor);
            _entities.Add(position, actor);
            return actor;
        }

        public static Actor RemoveAt(Point position)
        {
            if (_entities.TryGetValue(position, out Actor actor))
            {
                EntityComponent.Remove(actor);
                _entities.Remove(position);
                return actor;
            }
            return null;
        }

        public static bool ExistsAt(Point point)
        {
            return _entities.ContainsKey(point);
        }

        public static Actor GetEntityAt(Point point)
        {
            return _entities.TryGetValue(point, out Actor actor) ? actor : null;
        }

        public static void Update(Point previous, Point current)
        {
            if (_entities.TryGetValue(previous, out Actor actor))
            {
                if (_entities.ContainsKey(current))
                    throw new Exception("An entity already exists at this location.");

                if (actor.Position != current)
                    actor.Position = current;

                _entities.Remove(previous);
                _entities.Add(current, actor);
            }
            else
            {
                throw new Exception("No entity exists at this location.");
            }
        }
    }
}
