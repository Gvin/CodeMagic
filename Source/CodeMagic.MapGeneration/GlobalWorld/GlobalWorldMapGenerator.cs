using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Implementations.Objects.GlobalMap;

namespace CodeMagic.MapGeneration.GlobalWorld
{
    public class GlobalWorldMapGenerator
    {
        public const string LocationId = "world";
        private const int MinDistanceFromHome = 7;
        private const int MinDistanceFromDungeon = 5;

        public IAreaMap GenerateMap(int width, int height, out Point playerPosition)
        {
            var map = new GlobalAreaMap(width, height, new OutsideEnvironmentLightManager(), 2);

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (x == 0 || y == 0 || x == map.Width - 1 || y == map.Width - 1)
                    {
                        map.AddObject(x, y, new GlobalMapObject(GlobalMapObject.Type.Rock));
                        continue;
                    }

                    map.AddObject(x, y, GenerateMapObject());
                }
            }

            var homePosition = GenerateHomePosition(map);
            map.AddObject(homePosition, new HomeObject());

            playerPosition = GeneratePlayerPosition(homePosition);

            GenerateDungeons(map, homePosition);
            return map;
        }

        private void GenerateDungeons(IAreaMap map, Point homePosition)
        {
            var dungeonsCount = (int)Math.Round(map.Width * map.Height * 0.01);
            var dungeonsPoints = new List<Point>(dungeonsCount);

            for (int counter = 0; counter < dungeonsCount; counter++)
            {
                var position = GenerateDungeonPosition(map, homePosition, dungeonsPoints);
                dungeonsPoints.Add(position);

                var rareness = RandomHelper.GetRandomElement(Enum.GetValues(typeof(ItemRareness)).Cast<ItemRareness>().ToArray());
                map.AddObject(position, new DungeonEnterObject(rareness));
            }
        }

        private Point GenerateDungeonPosition(IAreaMap map, Point homePosition, List<Point> dungeonsPosition)
        {
            const int retryCount = 100;
            var iteration = 0;

            while (iteration < retryCount)
            {
                iteration++;

                var x = RandomHelper.GetRandomValue(2, map.Width - 2);
                var y = RandomHelper.GetRandomValue(2, map.Height - 2);

                if (!IsEmptyArea(map, x, y))
                    continue;

                var position = new Point(x, y);
                if (Point.GetDistance(homePosition, position) < MinDistanceFromHome)
                    continue;

                if (dungeonsPosition.Any(pos => Point.GetDistance(pos, position) < MinDistanceFromDungeon))
                    continue;

                return position;
            }

            throw new MapGenerationException("Unable to generate dungeon location.");
        }

        private Point GeneratePlayerPosition(Point homePoint)
        {
            var direction =
                RandomHelper.GetRandomElement(Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray());
            return Point.GetPointInDirection(homePoint, direction);
        }

        private Point GenerateHomePosition(IAreaMap map)
        {
            const int retryCount = 100;
            var iteration = 0;

            while (iteration < retryCount)
            {
                iteration++;

                var x = RandomHelper.GetRandomValue(2, map.Width - 2);
                var y = RandomHelper.GetRandomValue(2, map.Height - 2);

                if (!IsEmptyArea(map, x, y))
                    continue;

                return new Point(x, y);
            }

            throw new MapGenerationException("Unable to generate player home location.");
        }

        private bool IsEmptyArea(IAreaMap map, int x, int y)
        {
            if (map.GetCell(x, y).BlocksMovement)
                return false;

            if (map.GetCell(x - 1, y).BlocksMovement &&
                map.GetCell(x + 1, y).BlocksMovement &&
                map.GetCell(x, y - 1).BlocksMovement &&
                map.GetCell(x, y + 1).BlocksMovement)
                return false;

            return true;
        }

        private IMapObject GenerateMapObject()
        {
            if (RandomHelper.CheckChance(10))
            {
                return new GlobalMapObject(GlobalMapObject.Type.Dirt);
            }

            if (RandomHelper.CheckChance(10))
            {
                return new GlobalMapObject(GlobalMapObject.Type.Rock);
            }

            if (RandomHelper.CheckChance(30))
            {
                return new GlobalMapObject(GlobalMapObject.Type.Forest);
            }

            return new GlobalMapObject(GlobalMapObject.Type.Grass);
        }
    }
}