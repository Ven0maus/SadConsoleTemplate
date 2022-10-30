using SadConsoleTemplate.Entities;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace SadConsoleTemplate.Graphics
{
    internal class MapWindow : Console
    {
        public MapWindow(int width, int height) : base(width, height)
        {
            Surface.FillBorder('#', Color.White, Color.Transparent);
            SadComponents.Add(EntityManager.EntityComponent);
        }
    }
}
