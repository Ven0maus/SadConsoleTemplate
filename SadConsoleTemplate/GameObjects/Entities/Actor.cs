using GoRogue;
using Microsoft.Xna.Framework;
using SadConsoleTemplate.World;
using SadConsole.Entities;
using SadConsoleTemplate.GameObjects.Components;

namespace SadConsoleTemplate.GameObjects.Entities
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
        /// <summary>
        /// The radius of the field of view
        /// </summary>
        public int FieldOfViewRadius { get; set; }
        /// <summary>
        /// The component responsible for calculating the FieldOfView positions
        /// </summary>
        public FovComponent FieldOfView { get; private set; }

        /// <summary>
        /// Base constructor for an actor
        /// </summary>
        /// <param name="foreground"></param>
        /// <param name="background"></param>
        /// <param name="glyph"></param>
        /// <param name="position"></param>
        /// <param name="distanceCalculationMethod">Default MANHATTAN calculation</param>
        public Actor(Color foreground, Color background, int glyph, Coord position, Distance distanceCalculationMethod = null)
                    : base(foreground, background, glyph) => Initialize(position, (int)MapLayer.ENTITIES, distanceCalculationMethod);

        /// <summary>
        /// Initializes the basic properties of an entity
        /// </summary>
        /// <param name="position"></param>
        /// <param name="layer"></param>
        private void Initialize(Coord position, int layer, Distance distanceCalculationMethod)
        {
            FieldOfView = new FovComponent(this, distanceCalculationMethod ?? Distance.MANHATTAN);
            Position = position;
            Layer = layer;
        }

        /// <summary>
        /// Basic collision detection movement
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="movementCheck"></param>
        /// <returns></returns>
        public bool MoveTowards(int x, int y, bool movementCheck = true)
        {
            var targetPos = new Point(x, y);
            if (Position == targetPos) return false;

            bool canMove = !movementCheck;
            if (movementCheck)
            {
                canMove = Game.MapScreen.Map.GetCell(targetPos.X, targetPos.Y).IsWalkable;
            }

            if (canMove)
            {
                Position = targetPos;
            }
            return canMove;
        }

        /// <summary>
        /// Basic collision detection movement
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="movementCheck"></param>
        /// <returns></returns>
        public bool MoveTowards(Direction direction, bool movementCheck = true)
        {
            var pos = Position;
            var targetPos = pos += direction;
            return MoveTowards(targetPos.X, targetPos.Y, movementCheck);
        }
    }
}
