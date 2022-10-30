using SadConsoleTemplate.Entities;
using SadRogue.Primitives;
using System;
using Console = SadConsole.Console;

namespace SadConsoleTemplate.Graphics
{
    internal class GameWindow : Console
    {
        private static GameWindow _instance;
        public static GameWindow Instance { get { return _instance; } }
        public MapWindow MapWindow { get;}
        public PlayerWindow PlayerWindow { get; }
        public MessageWindow MessageWindow { get; }
        public Player Player { get; }

        public GameWindow() : base(Constants.Screens.GameScreen.Width, Constants.Screens.GameScreen.Height)
        {
            if (_instance != null)
                throw new Exception("A game window instance already exists, cannot create multiple!");
            _instance = this;

            // Initialize windows
            MapWindow = new MapWindow(Constants.Screens.MapScreen.Width, Constants.Screens.MapScreen.Height)
            {
                Position = new Point(0, 0)
            };
            PlayerWindow = new PlayerWindow(Constants.Screens.PlayerScreen.Width, Constants.Screens.PlayerScreen.Height)
            {
                Position = new Point(Constants.Screens.MapScreen.Width, 0)
            };
            MessageWindow = new MessageWindow(Constants.Screens.MessageScreen.Width, Constants.Screens.MessageScreen.Height)
            {
                Position = new Point(0, Constants.Screens.MapScreen.Height)
            };
            Children.Add(MapWindow);
            Children.Add(PlayerWindow);
            Children.Add(MessageWindow);

            // Initialize the player entity
            EntityManager.SpawnAt<Player>((Constants.Screens.MapScreen.Width / 2, Constants.Screens.MapScreen.Height / 2));
        }
    }
}
