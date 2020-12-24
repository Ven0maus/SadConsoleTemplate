using Microsoft.Xna.Framework;
using SadConsole;
using SadConsoleTemplate.World;
using SadConsoleTemplate.World.Settings;

namespace SadConsoleTemplate.Graphics
{
    public class WorldScreen : ScrollingConsole
    {
        public Grid World { get; }

        public WorldScreen(int width, int height, Font font, Rectangle viewPort) : base(width, height, font, viewPort)
        {
            World = CreateWorld();

            // Initialize the cells of the world onto the renderer
            World.InitializeRenderer();

            // Apply focus to the controlled entity, so keyboard input is received
            World.ControlledEntity.IsFocused = true;

            // Apply required events to the controlled entity
            World.ControlledEntity.Moved += ControlledEntity_Moved;
            World.ControlledEntityChanged += Map_ControlledEntityChanged;

            // Center viewport and calculate field of view
            ControlledEntity_Moved(null, null);
        }

        /// <summary>
        /// Creates the world grid based on the WorldGenSettings generation.
        /// </summary>
        /// <returns></returns>
        private Grid CreateWorld()
        {
            var map = new Grid(WorldGenSettings.WorldSizeWidth, WorldGenSettings.WorldSizeHeight, this);
            map.Generate(WorldGenSettings.WorldGeneration);
            return map;
        }

        private void Map_ControlledEntityChanged(object sender, ControlledEntityChangedArgs e)
        {
            // Replaces the controlled entity's moved event with the new controlled entity
            if (e.OldEntity != null)
                e.OldEntity.Moved -= ControlledEntity_Moved;
            World.ControlledEntity.Moved += ControlledEntity_Moved;
        }

        private void ControlledEntity_Moved(object sender, SadConsole.Entities.Entity.EntityMovedEventArgs e)
        {
            // Center the viewport of the camera onto the controlled entity
            this.CenterViewPortOnPoint(World.ControlledEntity.Position);
        }
    }
}
