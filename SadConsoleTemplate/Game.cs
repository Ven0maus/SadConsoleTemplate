using Microsoft.Xna.Framework;
using SadConsoleTemplate.Graphics;

namespace SadConsoleTemplate
{
    public class Game
    {
        public const int WindowWidth = 80;
        public const int WindowHeight = 25;

        public static WorldScreen WorldScreen { get; private set; }

        static void Main()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(WindowWidth, WindowHeight);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        /// <summary>
        /// Base initialize method to setup the WorldScreen as current screen
        /// </summary>
        private static void Init()
        {
            WorldScreen = new WorldScreen(new Rectangle(0, 0, WindowWidth, WindowHeight));
            SadConsole.Global.CurrentScreen = WorldScreen;
        }
    }
}
