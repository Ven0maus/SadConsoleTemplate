using SadConsole;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace SadConsoleTemplate.Graphics.Screens
{
    internal class MapScreen : Console
    {
        public const string Title = "World";
        public static Color TitleColor = Color.White;

        public MapScreen(int width, int height) : base(width, height)
        {
            // Draw borders
            var shapeParams = ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(Color.Gray), ignoreBorderBackground: true);
            Surface.DrawBox(new Rectangle(0, 0, width, height), shapeParams);

            // Print title
            var title = " " + Title + " ";
            Surface.Print(width / 2 - title.Length / 2, 0, new ColoredString(title, TitleColor, Color.Transparent));

            // Add entity component renderer to the map screen, for rendering entities
            SadComponents.Add(GameContainer.Instance.EntityManager.EntityComponent);
        }
    }
}
