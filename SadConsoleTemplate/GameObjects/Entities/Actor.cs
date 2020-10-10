using GoRogue;
using Microsoft.Xna.Framework;
using Mordred.World;
using SadConsole.Entities;

namespace Mordred.GameObjects.Entities
{
    /// <summary>
    /// A base template for an entity
    /// </summary>
    public abstract class Actor : Entity
    {
        /// <summary>
        /// The layer this entity is rendered on, (by default MapLayer.ENTITIES)
        /// </summary>
        public int Layer { get; private set; }
        public Actor(Color foreground, Color background, int glyph, Coord position)
                    : base(foreground, background, glyph) => Initialize(position, (int)MapLayer.ENTITIES);

        /// <summary>
        /// Initializes the basic properties of an entity
        /// </summary>
        /// <param name="position"></param>
        /// <param name="layer"></param>
        private void Initialize(Coord position, int layer)
        {
            Position = position;
            Layer = layer;
        }
    }
}
