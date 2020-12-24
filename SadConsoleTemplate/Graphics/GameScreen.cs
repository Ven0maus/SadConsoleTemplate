using GoRogue;
using SadConsole;

namespace SadConsoleTemplate.Graphics
{
    public class GameScreen : ContainerConsole
    {
        public WorldScreen WorldScreen { get; }

        public GameScreen(Rectangle viewport)
        {
            WorldScreen = new WorldScreen(Game.WindowWidth, Game.WindowHeight, Global.FontDefault, viewport)
            {
                Parent = this
            };
        }
    }
}
