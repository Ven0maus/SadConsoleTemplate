namespace SadConsoleTemplate
{
    internal static class Constants
    {
        /// <summary>
        /// If the game will never do anything concurrently off the main thread, this property should remain false.
        /// It will automatically assign the correct synchronized or concurrent implementation(s) for the base logic such as the EntityManager.
        /// </summary>
        public const bool UseConcurrentImplementations = false;

        public static class Screens
        {
            public const int GameContainerWidth = 100;
            public const int GameContainerHeight = 40;
            public const int MapScreenWidth = 70;
            public const int MapScreenHeight = 28;
            public const int PlayerScreenWidth = 30;
            public const int PlayerScreenHeight = 40;
            public const int MessageScreenWidth = 70;
            public const int MessageScreenHeight = 12;
        }
    }
}
