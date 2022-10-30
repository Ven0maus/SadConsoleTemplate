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

        public void MoveTowards(Point position, bool checkCanMove = true)
        {
            if (Health == 0) return;

            if (checkCanMove && !CanMoveTowards(position)) return;

            var oldPos = Position;
            Position = position;

            EntityManager.Update(oldPos, Position);
        }

        public void MoveTowards(Direction position, bool checkCanMove = true)
        {
            MoveTowards(Position += position, checkCanMove);
        }

        public bool CanMoveTowards(Point position)
        {
            return Health > 0 && !EntityManager.ExistsAt(position);
        }
    }
}
