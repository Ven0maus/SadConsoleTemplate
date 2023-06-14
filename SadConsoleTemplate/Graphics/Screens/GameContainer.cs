using SadConsole;
using SadConsoleTemplate.Entities;
using SadConsoleTemplate.Managers.Entities;
using SadRogue.Primitives;
using System;
using Console = SadConsole.Console;

namespace SadConsoleTemplate.Graphics.Screens
{
    internal class GameContainer : ScreenObject
    {
        private static GameContainer _instance;
        public static GameContainer Instance { get { return _instance; } }

        public MapScreen MapWindow { get; private set; }
        public PlayerScreen PlayerWindow { get; private set; }
        public MessageScreen MessageWindow { get; private set; }
        public EntityManager EntityManager { get; private set; }
        public Player Player { get; }

        /// <summary>
        /// Basic constructor for game container.
        /// </summary>
        /// <param name="asyncGame"></param>
        /// <exception cref="Exception"></exception>
        public GameContainer()
        {
            // Set singleton instance
            if (_instance != null)
                throw new Exception("A game window instance already exists, cannot create multiple!");
            _instance = this;

            // Initialize game managers
            InitManagers();

            // Initialize screens
            InitScreens();

            // Initialize player entity
            Player = EntityManager.CreateAt<Player>((Constants.Screens.MapScreenWidth / 2, Constants.Screens.MapScreenHeight / 2));
        }

        private void InitManagers()
        {
            EntityManager = Constants.UseConcurrentImplementations ? new ConcurrentEntityManager() : new SynchronizedEntityManager();
        }

        private void InitScreens()
        {
            MapWindow = new MapScreen(Constants.Screens.MapScreenWidth, Constants.Screens.MapScreenHeight) 
            { Position = new Point(0, 0) };
            Children.Add(MapWindow);

            PlayerWindow = new PlayerScreen(Constants.Screens.PlayerScreenWidth, Constants.Screens.PlayerScreenHeight) 
            { Position = new Point(Constants.Screens.MapScreenWidth, 0) };
            Children.Add(PlayerWindow);

            MessageWindow = new MessageScreen(Constants.Screens.MessageScreenWidth, Constants.Screens.MessageScreenHeight) 
            { Position = new Point(0, Constants.Screens.MapScreenHeight) };
            Children.Add(MessageWindow);
        }
    }
}
