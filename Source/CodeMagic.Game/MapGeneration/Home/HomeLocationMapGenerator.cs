using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Game.Area;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Objects.Floor;
using CodeMagic.Game.Objects.SolidObjects;

namespace CodeMagic.Game.MapGeneration.Home
{
    public class HomeLocationMapGenerator
    {
        public const string LocationId = "home";

        public IAreaMap GenerateMap(int width, int height, out Dictionary<Direction, Point> locationEnters, out Point playerPosition)
        {
            var map = new AreaMap(() => new GameEnvironment(ConfigurationManager.Current.Physics), width, height, new OutsideEnvironmentLightManager());

            locationEnters = new Dictionary<Direction, Point>
            {
                {Direction.North, new Point(width / 2 - 1, 0)},
                {Direction.South, new Point(width / 2 - 1, height - 1)},
                {Direction.East, new Point(width - 1, height / 2 - 1)},
                {Direction.West, new Point(0, height / 2 - 1)}
            };

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    map.AddObject(x, y, new FloorObject(FloorObject.Type.Grass));

                    if (!locationEnters.Values.Any(pos => pos.X == x && pos.Y == y) &&
                        RandomHelper.CheckChance(20))
                    {
                        map.AddObject(x, y, new Tree());
                    }
                }
            }

            

            playerPosition = new Point(1, 1);

            return map;
        }
    }
}