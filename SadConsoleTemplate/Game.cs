using SadConsoleTemplate.Graphics;

namespace SadConsoleTemplate
{
    public class Game
    {
        public const int Width = 80;
        public const int Height = 25;

        public static MapScreen MapScreen { get; private set; }

        static void Main()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        /// <summary>
        /// Base initialize method to setup the MapScreen as current screen
        /// </summary>
        private static void Init()
        {
            MapScreen = new MapScreen(Width, Height, Width, Height);
            SadConsole.Global.CurrentScreen = MapScreen;
        }
    }
}
