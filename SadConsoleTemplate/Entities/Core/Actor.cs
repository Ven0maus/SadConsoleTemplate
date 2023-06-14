using SadConsole.Entities;
using SadRogue.Primitives;

namespace SadConsoleTemplate.Entities.Core
{
    internal abstract class Actor : Entity
    {
        public Actor(int glyph, Color foreground, Color background, int zIndex = 1)
            : base(foreground, background, glyph, zIndex)
        { }

        /// <summary>
        /// Return true if the position is valid to move towards, false if invalid.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>True/False</returns>
        public virtual bool ValidateMovement(Point position)
        {
            return true;
        }

        /// <summary>
        /// Move actor towards the given position. With optional movement validation.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="validateMovement">Validate if movement can happen.</param>
        /// <returns>True if moved, false if not</returns>
        public bool MoveTowards(Point position, bool validateMovement = true)
        {
            if (validateMovement && !ValidateMovement(position)) return false;
            Position = position;
            return true;
        }

        /// <summary>
        /// Move actor towards the given direction. With optional movement validation.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="validateMovement">Validate if movement can happen.</param>
        /// <returns>True if moved, false if not</returns>
        public bool MoveTowards(Direction position, bool validateMovement = true)
        {
            return MoveTowards(Position + position, validateMovement);
        }
    }
}
