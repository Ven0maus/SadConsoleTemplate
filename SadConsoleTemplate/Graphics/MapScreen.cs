using GoRogue;
using SadConsoleTemplate.GameObjects.Entities;
using SadConsoleTemplate.World;
using SadConsole;

namespace SadConsoleTemplate.Graphics
{
    public class MapScreen : ContainerConsole
    {
        /// <summary>
        /// The grid associated with this MapScreen
        /// </summary>
        public Grid Map { get; }
        /// <summary>
        /// The rendering console responsible for rendering the grid associated with this MapScreen
        /// </summary>
        public ScrollingConsole MapRenderer { get; }

        public MapScreen(int width, int height, int viewportWidth, int viewportHeight)
        {
            Map = GenerateDungeon(width, height);

            // Create a rendering console, and add it to this container console as a child
            MapRenderer = Map.CreateRenderer(new Rectangle(0, 0, viewportWidth, viewportHeight), Global.FontDefault);
            Children.Add(MapRenderer);

            // Apply focus to the controlled entity, so keyboard input is received
            Map.ControlledEntity.IsFocused = true;

            // Apply required events to the controlled entity
            Map.ControlledEntity.Moved += ControlledEntity_Moved;
            Map.ControlledEntityChanged += Map_ControlledEntityChanged;

            // Center viewport and calculate field of view
            ControlledEntity_Moved(null, null);
        }

        private void Map_ControlledEntityChanged(object sender, ControlledEntityChangedArgs e)
        {
            // Replaces the controlled entity's moved event with the new controlled entity
            if (e.OldEntity != null)
                e.OldEntity.Moved -= ControlledEntity_Moved;
            Map.ControlledEntity.Moved += ControlledEntity_Moved;
        }

        private void ControlledEntity_Moved(object sender, SadConsole.Entities.Entity.EntityMovedEventArgs e)
        {
            // Calculate field of view
            if (Map.ControlledEntity is Actor actor)
            {
                actor.FieldOfView.Calculate();
            }

            // Center the viewport of the camera onto the controlled entity
            MapRenderer.CenterViewPortOnPoint(Map.ControlledEntity.Position);
        }

        /// <summary>
        /// Create's a basic dungeon grid
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static Grid GenerateDungeon(int width, int height)
        {
            var map = new Grid(width, height);

            // Set a basic grid
            map.SetBaseGrid();

            // Generate player
            var pos = new Coord(width / 2, height / 2);
            map.ControlledEntity = new Player(pos);
            map.AddEntity(map.ControlledEntity);

            return map;
        }
    }
}
