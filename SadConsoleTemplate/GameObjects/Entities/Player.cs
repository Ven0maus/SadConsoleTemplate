using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole.Components;
using System.Collections.Generic;

namespace SadConsoleTemplate.GameObjects.Entities
{
    /// <summary>
    /// The base class for the player entity
    /// </summary>
    public class Player : Actor
    {
        /// <summary>
        /// All the key mappings for the player movement
        /// </summary>
        private static readonly Dictionary<Keys, Direction> s_movementDirectionMapping = new Dictionary<Keys, Direction>
        {
            { Keys.Z, Direction.UP }, { Keys.S, Direction.DOWN },
            { Keys.Q, Direction.LEFT }, { Keys.D, Direction.RIGHT }
        };

        public Player(Coord position) : base(Color.White, Color.Transparent, '@', position)
        {
            FieldOfViewRadius = 8;

            // Incase our viewport != size of game screen
            // This automatically sync's the camera view
            Components.Add(new EntityViewSyncComponent());
        }

        public bool ProcessMovement(SadConsole.Input.Keyboard info)
        {
            Direction moveDirection = Direction.NONE;

            // Simplified way to check if any key we care about is pressed and set movement direction.
            foreach (Keys key in s_movementDirectionMapping.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    moveDirection = s_movementDirectionMapping[key];
                    break;
                }
            }

            var moved = MoveTowards(moveDirection);

            if (moved)
                return true;
            else
                return false;
        }
    }
}
