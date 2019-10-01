using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
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

        private const double RocksCountFactor = 0.05d;
        private const double RocksSizeFactor = 0.001d;

        private const double ForestsCountFactor = 0.07;
        private const double ForestsSizeFactor = 0.001d;

        public IAreaMap GenerateMap(int width, int height, out Point playerHomePosition)
        {
            var map = new GlobalAreaMap(width, height, new OutsideEnvironmentLightManager(), 2);

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (x == 0 || y == 0 || x == map.Width - 1 || y == map.Width - 1) // Adding borders
                    {
                        map.AddObject(x, y, new GlobalMapObject(GlobalMapObject.Type.Rock));
                        continue;
                    }

                    map.AddObject(x, y, GenerateMapFloor());
                }
            }

            PlacePatches(map, () => new GlobalMapObject(GlobalMapObject.Type.Rock), RocksCountFactor, RocksSizeFactor);
            PlacePatches(map, () => new GlobalMapForest(), ForestsCountFactor, ForestsSizeFactor);


            playerHomePosition = GenerateHomePosition(map);
            map.AddObject(playerHomePosition, new HomeObject());

            GenerateDungeons(map, playerHomePosition);
            return map;
        }

        private void PlacePatches(IAreaMap map, Func<IMapObject> factory, double countFactor, double sizeFactor)
        {
            var mapSize = map.Width * map.Height;
            var count = (int) Math.Round(mapSize * countFactor);
            var maxSize = (int) Math.Round(mapSize * sizeFactor);

            for (int counter = 0; counter < count; counter++)
            {
                var size = RandomHelper.GetRandomValue(2, maxSize);
                PlacePatch(map, factory, size);
            }
        }

        private void PlacePatch(IAreaMap map, Func<IMapObject> factory, int size)
        {
            var startX = RandomHelper.GetRandomValue(0, map.Width - 1);
            var startY = RandomHelper.GetRandomValue(0, map.Height - 1);

            for (var radius = 0; radius <= size / 2; radius++)
            {
                for (int y = -radius; y < radius; y++)
                {
                    for (int x = -radius; x < radius; x++)
                    {
                        var point = new Point(startX + x, startY + y);
                        var distance = Point.GetDistance(new Point(startX, startY), point);
                        if (distance > radius)
                            continue;

                        if (!map.ContainsCell(point))
                            continue;

                        if (map.GetCell(point).Objects.Any(obj => obj.ZIndex != ZIndex.Floor))
                            continue;

                        var chance = (int)Math.Round((radius - distance + 1) / radius * 100);
                        if (RandomHelper.CheckChance(chance))
                        {
                            map.AddObject(point, factory());
                        }
                    }
                }
            }
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
            if (map.GetCell(x, y).Objects.OfType<GlobalMapForest>().Any())
                return false;

            if (map.GetCell(x, y).BlocksMovement)
                return false;

            if (map.GetCell(x - 1, y).BlocksMovement ||
                map.GetCell(x + 1, y).BlocksMovement ||
                map.GetCell(x, y - 1).BlocksMovement ||
                map.GetCell(x, y + 1).BlocksMovement)
                return false;

            return true;
        }

        private IMapObject GenerateMapFloor()
        {
            if (RandomHelper.CheckChance(10))
            {
                return new GlobalMapObject(GlobalMapObject.Type.Dirt);
            }

            return new GlobalMapObject(GlobalMapObject.Type.Grass);
        }
    }
}