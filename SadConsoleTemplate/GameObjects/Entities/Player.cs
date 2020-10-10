using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Mordred.GameObjects.Entities
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
        { }

        /// <summary>
        /// Automatically called by sadconsole to process keyboard input
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
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

            Position += moveDirection;

            if (moveDirection != Direction.NONE)
                return true;
            else
                return base.ProcessKeyboard(info);
        }
    }
}
