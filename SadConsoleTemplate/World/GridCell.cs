using Microsoft.Xna.Framework;
using SadConsole;

namespace SadConsoleTemplate.World
{
    /// <summary>
    /// Base class of a grid cell
    /// </summary>
    public class GridCell : Cell
    {
        /// <summary>
        /// Can entities walk on this cell
        /// </summary>
        public bool IsWalkable { get; set; }
        /// <summary>
        /// Can entities see through this cell
        /// </summary>
        public bool IsTransparent { get; set; }
        /// <summary>
        /// The layer this cell is build on
        /// </summary>
        public int Layer { get; private set; }

        public GridCell(Color foreground, Color background, int glyph, int layer, bool isWalkable, bool isTransparent) : 
            base(foreground, background, glyph)
        {
            IsWalkable = isWalkable;
            IsTransparent = isTransparent;
            Layer = layer;
        }

        public GridCell(GridCell cell) : base(cell.Foreground, cell.Background, cell.Glyph)
        {
            IsWalkable = cell.IsWalkable;
            IsTransparent = cell.IsTransparent;
            Layer = cell.Layer;
        }

        /// <summary>
        /// Replaces all the data of this cell with the given cell's data
        /// </summary>
        /// <param name="cell"></param>
        public void Replace(GridCell cell)
        {
            // Copy base data
            CopyAppearanceFrom(cell);

            // Copy custom data
            IsWalkable = cell.IsWalkable;
            IsTransparent = cell.IsTransparent;
            Layer = cell.Layer;
        }
    }
}
