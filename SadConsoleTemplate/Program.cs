using SadConsoleTemplate.Graphics;
using SadConsole;

namespace SadConsoleTemplate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            Game.Create(Constants.Screens.GameScreen.Width, Constants.Screens.GameScreen.Height);

            // Hook the start event so we can add consoles to the system.
            Game.Instance.OnStart = OnInitialization;
            Game.Instance.FrameUpdate += OnFrameUpdate;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void OnInitialization()
        {
            Game.Instance.Screen = new GameWindow();
        }

        private static void OnFrameUpdate(object sender, GameHost e)
        {

        }
    }
}