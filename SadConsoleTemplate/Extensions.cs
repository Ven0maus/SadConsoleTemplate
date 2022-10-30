using SadConsole;
using SadRogue.Primitives;

namespace SadConsoleTemplate
{
    internal static class Extensions
    {
        public static void FillBorder(this ICellSurface surface, int glyph, Color foreground, Color background)
        {
            for (int x = 0; x < surface.Width; x++)
                for (int y = 0; y < surface.Height; y++)
                    if (x == 0 || y == 0 || x == surface.Width - 1 || y == surface.Height - 1)
                        surface.SetGlyph(x, y, glyph, foreground, background);
        }
    }
}
