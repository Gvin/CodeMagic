using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.Creatures.Implementations;
using CodeMagic.Implementations.Objects.Creatures.NonPlayable;

namespace CodeMagic.MapGeneration
{
    internal class NpcGenerator
    {
        private const int SkeletonsCountMultiplier = 10;
        private const int MaxSkeletonsCount = 50;
        private const int MinDistanceFromPlayer = 6;

        public void GenerateNpc(int level, MapSize mapSize, IAreaMap map, Point playerPosition)
        {
            var count = level * SkeletonsCountMultiplier + (int) mapSize * 10;
            count = Math.Min(MaxSkeletonsCount, count);
            PlaceCreatures(count, map, playerPosition);
        }

        private void PlaceCreatures(int count, IAreaMap map, Point playerPosition)
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

                var npc = CreateNpc();
                map.AddObject(position, npc);
                placed++;
            }
        }

        private bool CheckDistanceFromPlayer(Point playerPosition, Point targetPosition)
        {
            return Point.GetDistance(playerPosition, targetPosition) > MinDistanceFromPlayer;
        }

        private NonPlayableCreatureObject CreateNpc()
        {
            return CreateGoblin();
        }

        private NonPlayableCreatureObject CreateGoblin()
        {
            return new SkeletonImpl(new SkeletonCreatureObjectConfiguration
            {
                Name = "Skeleton",
                Health = 10,
                MaxHealth = 10,
                MinDamage = 1,
                MaxDamage = 4,
                HitChance = 60,
                VisibilityRange = 3
            });
        }
    }
}