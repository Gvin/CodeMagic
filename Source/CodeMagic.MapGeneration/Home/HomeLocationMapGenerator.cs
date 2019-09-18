using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Implementations.Objects;
using CodeMagic.Implementations.Objects.Floor;

namespace CodeMagic.MapGeneration.Home
{
    public class HomeLocationMapGenerator
    {
        public const string LocationId = "home";

        public IAreaMap GenerateMap(int width, int height, out Point playerPosition)
        {
            var map = new AreaMap(width, height, new OutsideEnvironmentLightManager());

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    map.AddObject(x, y, new FloorObject(FloorObject.Type.Grass));

                    if (x == 0 || y == 0 || x == map.Width - 1 || y == map.Height - 1)
                    {
                        map.AddObject(x, y, new LocationExitTriggerObject());
                    }
                }
            }

            playerPosition = new Point(2, 2);

            return map;
        }
    }
}