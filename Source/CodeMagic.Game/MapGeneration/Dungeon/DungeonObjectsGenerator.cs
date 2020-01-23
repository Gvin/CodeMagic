using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Game.Items.Materials;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.MapGeneration.Dungeon
{
    internal class DungeonObjectsGenerator
    {
        private const double StonesCountMultiplier = 0.05;
        private const double TorchPostsCountMultiplier = 0.01;

        private const int MaxPositionSearchTries = 20;

        public void GenerateObjects(IAreaMap map, bool addTorchPosts)
        {
            var stonesCount = (int) Math.Round(map.Width * map.Height * StonesCountMultiplier);
            AddStones(map, stonesCount);

            if (addTorchPosts)
            {
                var torchPostsCount = (int) Math.Round(map.Width * map.Height * TorchPostsCountMultiplier);
                AddTorchPosts(map, torchPostsCount);
            }
        }

        private void AddTorchPosts(IAreaMap map, int count)
        {
            for (int counter = 0; counter < count; counter++)
            {
                var position = GetFreePosition(map);
                if (position == null)
                    continue;

                map.AddObject(position, new DungeonTorchPost());
            }
        }

        private void AddStones(IAreaMap map, int stonesCount)
        {
            for (int counter = 0; counter < stonesCount; counter++)
            {
                var position = GetFreePosition(map);
                if (position == null)
                    continue;

                map.AddObject(position, new Stone());
            }
        }

        private Point GetFreePosition(IAreaMap map)
        {
            for (int counter = 0; counter < MaxPositionSearchTries; counter++)
            {
                var randomX = RandomHelper.GetRandomValue(0, map.Width - 1);
                var randomY = RandomHelper.GetRandomValue(0, map.Height - 1);

                var position = new Point(randomX, randomY);
                if (!map.GetCell(position).BlocksMovement)
                    return position;
            }
            return null;
        }
    }
}