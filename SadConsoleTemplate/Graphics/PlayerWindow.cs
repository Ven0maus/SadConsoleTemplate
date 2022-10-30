using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace SadConsoleTemplate.Graphics
{
    internal class PlayerWindow : Console
    {
        public PlayerWindow(int width, int height) : base (width, height)
        {
            Surface.FillBorder('#', Color.Green, Color.Transparent);
        }
    }
}
