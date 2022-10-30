using SadConsole.Input;
using SadRogue.Primitives;
using System.Collections.Generic;

namespace SadConsoleTemplate.Entities
{
    internal sealed class Player : Actor
    {
        public Player() : base('@', Color.Red, Color.Transparent)
        {
            IsFocused = true;
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
                    MoveTowards(moveDirection);
                    break;
                }
            }
            return base.ProcessKeyboard(keyboard);
        }
    }
}
