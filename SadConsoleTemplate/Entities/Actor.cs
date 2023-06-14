using SadConsole.Entities;
using SadRogue.Primitives;

namespace SadConsoleTemplate.Entities
{
    internal abstract class Actor : Entity
    {
        private int _maxHealth;
        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                var diff = value - _maxHealth;
                _maxHealth = value;
                Health += diff;
            }
        }

        public int Health { get; set; }
        public int Level { get; set; }

        public Actor(int glyph, Color foreground, Color background, int zIndex = 1) 
            : base(foreground, background, glyph, zIndex)
        { }

        public bool MoveTowards(Point position, bool checkCanMove = true)
        {
            if (Health == 0) return false;

            if (checkCanMove && !CanMoveTowards(position)) return false;

            var oldPos = Position;
            Position = position;

            EntityManager.Update(oldPos, Position);
            return true;
        }

        public bool MoveTowards(Direction position, bool checkCanMove = true)
        {
            return MoveTowards(Position + position, checkCanMove);
        }

        public bool CanMoveTowards(Point position)
        {
            return Health > 0 && !EntityManager.ExistsAt(position);
        }
    }
}
