using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Entities;
using SadConsoleTemplate.GameObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SadConsoleTemplate.World
{
    /// <summary>
    /// All the valid map layers
    /// </summary>
    internal enum MapLayer
    {
        TERRAIN,
        ITEMS,
        ENTITIES,
    }

    /// <summary>
    /// Base class of a world, represented by a square grid
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// The cells directly rendered to the renderConsole's surface.
        /// </summary>
        private readonly GridCell[] _cells;
        /// <summary>
        /// The entities directly rendered to the renderConsole's surface.
        /// </summary>
        private readonly List<Entity> _entities;
        /// <summary>
        /// Represents the console this grid is rendered to.
        /// </summary>
        private SadConsole.Console _renderConsole;

        /// <summary>
        /// Contains all positions in the grid, where true is a position that isTransparent and false !isTransparent
        /// </summary>
        public ArrayMap<bool> FieldOfView { get; }

        public int Width { get; }
        public int Height { get; }

        private Entity _controlledEntity;
        /// <summary>
        /// The entity that has input focus on this grid.
        /// </summary>
        public Entity ControlledEntity
        {
            get { return _controlledEntity; }
            set
            {
                if (_controlledEntity != value)
                {
                    var oldEntity = _controlledEntity;
                    _controlledEntity = value;
                    ControlledEntityChanged?.Invoke(this, new ControlledEntityChangedArgs(oldEntity));
                }
            }
        }

        /// <summary>
        /// The event fired when the ControlledEntity property is changed.
        /// </summary>
        public event EventHandler<ControlledEntityChangedArgs> ControlledEntityChanged;

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new GridCell[width * height];
            _entities = new List<Entity>();
            FieldOfView = new ArrayMap<bool>(Width, Height);
        }

        /// <summary>
        /// Create's a very basic grid, with walls around the grid.
        /// </summary>
        public void SetBaseGrid()
        {
            for (int y=0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (x != 0 && y != 0 && x != Width - 1 && y != Height - 1)
                        SetCell(x, y, new GridCell(Color.BlanchedAlmond, Color.Black, ' ', (int)MapLayer.TERRAIN, true, true));
                    else
                        SetCell(x, y, new GridCell(Color.DarkGray, Color.Black, '#', (int)MapLayer.TERRAIN, false, false));
                }
            }
        }

        /// <summary>
        /// The base method to set a new cell onto the grid.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cell"></param>
        public void SetCell(int x, int y, GridCell cell)
        {
            var oldCell = _cells[y * Width + x];
            // Update field of view, if it has changed.
            if (oldCell == null || cell == null || oldCell.IsTransparent != cell.IsTransparent)
            {
                FieldOfView[y * Width + x] = cell != null && cell.IsTransparent;
            }

            // Replace cell's properties if it exists, so it directly impacts the renderer's reference to our cells
            // If we assigned a new cell, the reference would be lost and we would need to call _renderConsole.SetSurface again
            // Else condition exists for cell initialization before CreateRenderer is called
            if (oldCell != null)
                _cells[y * Width + x].Replace(cell);
            else
                _cells[y * Width + x] = cell;
        }

        /// <summary>
        /// The base method to retrieve a cell on the grid.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GridCell GetCell(int x, int y)
        {
            // Return a copy of itself, instead of a direct reference to the original GridCell
            // This to prevent modifying the GridCell properties directly without using SetCell to push changes.
            return new GridCell(_cells[y * Width + x]);
        }

        /// <summary>
        /// Create's a rendering console for this grid to be rendered on.
        /// </summary>
        /// <param name="viewport"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public ScrollingConsole CreateRenderer(Rectangle viewport, Font font)
        {
            if (_cells.Any(a => a == null)) 
                throw new Exception("Please initialize the grid first.");

            var renderer = new ScrollingConsole(Width, Height, font, viewport, _cells);
            _renderConsole = renderer;

            renderer.SetSurface(_cells, Width, Height);
            renderer.IsDirty = true;

            foreach (var entity in _entities)
                renderer.Children.Add(entity);

            return renderer;
        }

        /// <summary>
        /// Add's an entity to be rendered by the grid.
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(Entity entity)
        {
            // Initialize field of view
            if (entity is Actor actor)
            {
                actor.FieldOfView.Initialize(FieldOfView);
            }

            if (_renderConsole != null)
            {
                _renderConsole.Children.Add(entity);
                _renderConsole.IsDirty = true;
            }
            _entities.Add(entity);
        }

        /// <summary>
        /// Removes an entity from the grid and it will no longer be rendered.
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveEntity(Entity entity)
        {
            // Reset field of view
            if (entity is Actor actor)
            {
                actor.FieldOfView.Reset();
            }

            if (_renderConsole != null)
            {
                var count = _renderConsole.Children.Count;
                _renderConsole.Children.Remove(entity);
                if (count != _renderConsole.Children.Count)
                    _renderConsole.IsDirty = true;
            }
            _entities.Remove(entity);
        }
    }

    /// <summary>
    /// The base event args for the ControlledEntityChanged event
    /// </summary>
    public class ControlledEntityChangedArgs : EventArgs
    {
        /// <summary>
        /// The previous entity before change.
        /// </summary>
        public Entity OldEntity;

        public ControlledEntityChangedArgs(Entity oldEntity)
        {
            OldEntity = oldEntity;
        }
    }
}
