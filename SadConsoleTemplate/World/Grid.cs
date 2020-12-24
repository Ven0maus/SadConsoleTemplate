using GoRogue;
using GoRogue.MapViews;
using GoRogue.Pathing;
using Microsoft.Xna.Framework;
using SadConsole.Entities;
using SadConsoleTemplate.GameObjects.Entities;
using SadConsoleTemplate.World.Generation;
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
        private readonly SadConsole.Console _renderConsole;

        /// <summary>
        /// Contains all positions in the grid, where true is a position that isTransparent and false !isTransparent
        /// </summary>
        public ArrayMap<bool> FieldOfView { get; }
        /// <summary>
        /// Contains all positions in the grid, where true is a position that isWalkable and false !isWalkable
        /// </summary>
        public ArrayMap<bool> Walkability { get; }
        /// <summary>
        /// Basic pathfinder, that chooses path's based on performance not on shortest length.
        /// </summary>
        public FastAStar PathFinder { get; }

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

        public Grid(int width, int height, SadConsole.Console screen)
        {
            _renderConsole = screen;
            Width = width;
            Height = height;
            _cells = new GridCell[width * height];
            _entities = new List<Entity>();
            FieldOfView = new ArrayMap<bool>(Width, Height);
            Walkability = new ArrayMap<bool>(Width, Height);
            PathFinder = new FastAStar(Walkability, Distance.MANHATTAN);
        }

        public void Generate(params Generator[] generators)
        {
            foreach (var gen in generators)
                gen.Execute(this);
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

            // Update field of view / walkability, if it has changed.
            FieldOfView[y * Width + x] = cell != null && cell.IsTransparent;
            Walkability[y * Width + x] = cell != null && cell.IsWalkable && GetEntityAt(x, y) == null;

            // Replace cell's properties if it exists, so it directly impacts the renderer's reference to our cells
            // If we assigned a new cell, the reference would be lost and we would need to call _renderConsole.SetSurface again
            // Else condition exists for cell initialization before CreateRenderer is called
            if (oldCell != null)
                _cells[y * Width + x].Replace(cell);
            else
                _cells[y * Width + x] = cell;
        }

        /// <summary>
        /// Handles pathfinding walkability view syncing for entities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnEntityMoved(object sender, Entity.EntityMovedEventArgs args)
        {
            // Reset from position
            Walkability[args.FromPosition.Y * Width + args.FromPosition.X] = true;
            // Set new position
            Walkability[args.Entity.Position.Y * Width + args.Entity.Position.X] = false;
            
            // Recalculate field of view
            if (args.Entity is Actor actor)
            {
                actor.FieldOfView.Calculate();
            }
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
        /// Returns the entity at that position in the map.
        /// Returns null if no entity at the given position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Entity GetEntityAt(int x, int y)
        {
            var position = new Point(x, y);
            var entity = _entities.FirstOrDefault(a => a.Position == position); 
            return entity;
        }

        /// <summary>
        /// Create's a rendering console for this grid to be rendered on.
        /// </summary>
        /// <param name="viewport"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public void InitializeRenderer()
        {
            if (_cells.Any(a => a == null)) 
                throw new Exception("Please initialize the grid first.");

            _renderConsole.SetSurface(_cells, Width, Height);
            _renderConsole.IsDirty = true;

            foreach (var entity in _entities)
                _renderConsole.Children.Add(entity);
        }

        /// <summary>
        /// Add's an entity to be rendered by the grid.
        /// Returns true if succesful, false if position was occupied by another entity.
        /// </summary>
        /// <param name="entity"></param>
        public bool AddEntity(Entity entity)
        {
            if (_entities.Any(a => a.Position == entity.Position))
                return false;

            // Initialize field of view
            if (entity is Actor actor)
            {
                actor.FieldOfView.Initialize(FieldOfView);
                actor.FieldOfView.Calculate();
            }

            if (_renderConsole != null)
            {
                _renderConsole.Children.Add(entity);
                _renderConsole.IsDirty = true;
            }
            entity.Moved += OnEntityMoved;
            _entities.Add(entity);
            return true;
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
            entity.Moved -= OnEntityMoved;
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
