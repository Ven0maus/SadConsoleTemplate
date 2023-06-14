using SadConsole;
using SadConsoleTemplate.Entities;
using SadRogue.Primitives;

namespace SadConsoleTemplate.Components
{
    internal class HealthBarComponent : ScreenSurface
    {
        public enum Direction
        {
            Horizontal,
            Vertical
        }

        private int _maxHealth;
        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; AdjustHealth(); }
        }

        private int _health;
        public int Health
        {
            get { return _health; }
            set { _health = value; AdjustHealth(); }
        }

        private readonly Actor _actor;
        private readonly Direction _direction;

        public HealthBarComponent(Actor actor, int size, Direction direction, int thickness = 1) 
            : base(direction == Direction.Horizontal ? size : thickness, direction == Direction.Vertical ? size : thickness)
        {
            _actor = actor;
            _direction = direction;
        }

        private void AdjustHealth()
        {
            if (Health < 0) Health = 0;
            if (MaxHealth < 0) MaxHealth = 0;
            if (Health > MaxHealth) Health = MaxHealth;

            // Right now it draws the full bar always
            // TODO: Color correct amount based on health

            if (_direction == Direction.Horizontal)
            {
                for (int size = 0; size < Surface.Width; size++)
                    for (int thickness=0; thickness < Surface.Height; thickness++)
                    Surface.SetBackground(size, thickness, Color.Red);
            }
            else
            {
                for (int size = 0; size < Surface.Height; size++)
                    for (int thickness = 0; thickness < Surface.Width; thickness++)
                        Surface.SetBackground(thickness, size, Color.Red);
            }
        }

        public void UpdatePosition()
        {
            if (Parent == null)
                throw new System.Exception("This component must be assigned to a parent first.");

            // Centers the position with 1 cell offset above or to the left of the actor
            var position = _actor.Position;
            Position = _direction == Direction.Horizontal ?
                new Point(position.X - Surface.Width / 2, position.Y - 1) :
                new Point(position.X - 1, position.Y - Surface.Height / 2);
        }
    }
}
