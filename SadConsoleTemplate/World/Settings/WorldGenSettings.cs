using SadConsoleTemplate.World.Generation;
using SadConsoleTemplate.World.Generation.Implementations;

namespace SadConsoleTemplate.World.Settings
{
    public static class WorldGenSettings
    {
        public const int WorldSizeWidth = 80;
        public const int WorldSizeHeight = 25;
        public static Generator[] WorldGeneration = new[] 
        { 
            new EmptyWorldGen() 
        };
    }
}
