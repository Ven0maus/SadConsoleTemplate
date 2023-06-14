using SadConsole;
using SadConsoleTemplate.Graphics.Screens;

namespace SadConsoleTemplate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Settings.WindowTitle = "SadConsoleTemplate";

            Game.Configuration gameStartup = new Game.Configuration()
                .SetScreenSize(Constants.Screens.GameContainerWidth, Constants.Screens.GameContainerHeight)
                .OnStart(OnInitialization)
                .UseFrameUpdateEvent(OnFrameUpdate)
                .SetStartingScreen(a => new GameContainer())
                .IsStartingScreenFocused(false)
                .ConfigureFonts((f) => f.UseBuiltinFontExtended());

            Game.Create(gameStartup);
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void OnInitialization()
        {
            // INIT CODE
        }

        private static void OnFrameUpdate(object sender, GameHost e)
        {
            // EACH FRAME CODE
        }
    }
}