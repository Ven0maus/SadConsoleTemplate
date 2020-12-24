using GoRogue;
using Microsoft.Xna.Framework;
using SadConsoleTemplate.GameObjects.Entities;

namespace SadConsoleTemplate.World.Generation.Implementations
{
    public class EmptyWorldGen : Generator
    {
        public override void Execute(Grid grid)
        {
            CreateWalledInGrid(grid);
            CreatePlayer(grid);
        }

        private void CreateWalledInGrid(Grid grid)
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

        private void CreatePlayer(Grid grid)
        {
            var pos = new Coord(grid.Width / 2, grid.Height / 2);
            grid.ControlledEntity = new Player(pos);
            grid.AddEntity(grid.ControlledEntity);
        }
    }
}
