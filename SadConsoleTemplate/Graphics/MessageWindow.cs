using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace SadConsoleTemplate.Graphics
{
    internal class MessageWindow : Console
    {
        public MessageWindow(int width, int height) : base(width, height)
        {
            Surface.FillBorder('#', Color.Blue, Color.Transparent);
        }
    }
}
