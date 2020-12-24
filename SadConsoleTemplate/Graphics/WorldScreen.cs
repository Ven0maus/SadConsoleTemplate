using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsoleTemplate.World;
using SadConsoleTemplate.World.Settings;

namespace SadConsoleTemplate.Graphics
{
    public class WorldScreen : ScrollingConsole
    {
        public Grid World { get; }

        public WorldScreen(int width, int height, Font font, Rectangle viewPort) : base(width, height, font, viewPort)
        {
            // Initialize world grid
            World = new Grid(WorldGenSettings.WorldSizeWidth, WorldGenSettings.WorldSizeHeight, this);

            // Generate world cells based on world gen settings
            World.Generate(WorldGenSettings.WorldGeneration);

            // Initialize the cells of the world onto the renderer
            World.InitializeRenderer();

            // Apply required events to the controlled entity
            World.ControlledEntity.Moved += ControlledEntity_Moved;
            World.ControlledEntityChanged += Map_ControlledEntityChanged;

            // Center viewport
            this.CenterViewPortOnPoint(World.ControlledEntity.Position);

            // Apply focus to the world screen
            IsFocused = true;
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

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (World.ControlledEntity == null) return false;
            return World.ControlledEntity.ProcessMovement(info);
        }
    }
}
