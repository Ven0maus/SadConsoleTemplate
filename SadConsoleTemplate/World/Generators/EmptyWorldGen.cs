using Microsoft.Xna.Framework;

namespace SadConsoleTemplate.World.Generators
{
    public class EmptyWorldGen : WorldGen
    {
        public override void Execute(Grid grid)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (x != 0 && y != 0 && x != grid.Width - 1 && y != grid.Height - 1)
                        grid.SetCell(x, y, new GridCell(Color.BlanchedAlmond, Color.Black, ' ', (int)MapLayer.TERRAIN, true, true));
                    else
                        grid.SetCell(x, y, new GridCell(Color.DarkGray, Color.Black, '#', (int)MapLayer.TERRAIN, false, false));
                }
            }
        }
    }
}
