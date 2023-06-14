using SadConsole;
using SadConsole.Input;
using SadConsoleTemplate.Components;
using SadRogue.Primitives;
using System.Collections.Generic;

namespace SadConsoleTemplate.Entities
{
    internal sealed class Player : Actor
    {
        private HealthBarComponent _healthBarComponent;

        public Player() : base('@', Color.Blue, Color.Transparent)
        {
            IsFocused = true;
            MaxHealth = 100;
        }

        public void AddHealthBarComponent(ScreenSurface surface)
        {
            _healthBarComponent = new HealthBarComponent(this, 5, HealthBarComponent.Direction.Vertical)
            {
                MaxHealth = MaxHealth,
                Health = MaxHealth
            };

            surface.Children.Add(_healthBarComponent);
            _healthBarComponent.UpdatePosition();
        }

        private readonly Dictionary<Keys, Direction> _playerMovements = new()
        {
            {Keys.Z, Direction.Up},
            {Keys.S, Direction.Down},
            {Keys.Q, Direction.Left},
            {Keys.D, Direction.Right}
        };

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            foreach (var key in _playerMovements.Keys)
            {
                if (keyboard.IsKeyPressed(key))
                {
                    var moveDirection = _playerMovements[key];
                    if (MoveTowards(moveDirection))
                        _healthBarComponent?.UpdatePosition();
                    break;
                }
            }
            return base.ProcessKeyboard(keyboard);
        }
    }
}
