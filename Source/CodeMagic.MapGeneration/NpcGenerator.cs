using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.MapGeneration
{
    internal class NpcGenerator
    {
        private const int MaxMobsCount = 100;
        private const int MinDistanceFromPlayer = 6;

        private static readonly Dictionary<MapType, double> MapTypeCountMultiplier = new Dictionary<MapType, double>
        {
            {MapType.Labyrinth, 20d},
            {MapType.Dungeon, 60d}
        };
        private static readonly Dictionary<MapSize, double> MapSizeCountMultiplier = new Dictionary<MapSize, double>
        {
            {MapSize.Small, 0.5d},
            {MapSize.Medium, 1d},
            {MapSize.Big, 2d}
        };

        private readonly MonstersGenerator monstersGenerator;

        public NpcGenerator()
        {
            monstersGenerator = new MonstersGenerator();
        }

        public void GenerateNpc(int level, MapSize mapSize, MapType mapType, IAreaMap map, Point playerPosition)
        {
            var count = (int)Math.Round(level * MapTypeCountMultiplier[mapType] * MapSizeCountMultiplier[mapSize]);
            count = Math.Min(MaxMobsCount, count);

            PlaceCreatures(count, level, map, playerPosition);
        }

        private void PlaceCreatures(int count, int level, IAreaMap map, Point playerPosition)
        {
            var maxIterations = count * count;
            var iteration = 0;
            var placed = 0;
            while (placed < count && iteration < maxIterations)
            {
                iteration++;

                var x = RandomHelper.GetRandomValue(0, 30);
                var y = RandomHelper.GetRandomValue(0, 30);
                var position = new Point(x, y);

                if (!CheckDistanceFromPlayer(playerPosition, position))
                    continue;

                var cell = map.TryGetCell(position);
                if (cell == null)
                    continue;

                if (cell.BlocksMovement)
                    continue;

                var npc = CreateMonster(level);
                map.AddObject(position, npc);
                placed++;
            }
        }

        private bool CheckDistanceFromPlayer(Point playerPosition, Point targetPosition)
        {
            return Point.GetDistance(playerPosition, targetPosition) > MinDistanceFromPlayer;
        }

        private INonPlayableCreatureObject CreateMonster(int level)
        {
            return monstersGenerator.GenerateRandomMonster(level);
        }
    }
}