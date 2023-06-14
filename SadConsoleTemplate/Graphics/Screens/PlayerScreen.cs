using SadConsole;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace SadConsoleTemplate.Graphics.Screens
{
    internal class PlayerScreen : Console
    {
        public const string Title = "Player Stats";
        public static Color TitleColor = Color.White;

        public PlayerScreen(int width, int height) : base(width, height)
        {
            // Draw borders
            var shapeParams = ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(Color.Green), ignoreBorderBackground: true);
            Surface.DrawBox(new Rectangle(0, 0, width, height), shapeParams);

            // Print title
            var title = " " + Title + " ";
            Surface.Print(width / 2 - title.Length / 2, 0, new ColoredString(title, TitleColor, Color.Transparent));
        }
    }
}
