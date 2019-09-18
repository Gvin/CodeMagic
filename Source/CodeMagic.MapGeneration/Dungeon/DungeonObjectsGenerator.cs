using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.SolidObjects;

namespace CodeMagic.MapGeneration.Dungeon
{
    internal class DungeonObjectsGenerator
    {
        private const double StonesCountMultiplier = 0.1;
        private const int MaxPositionSearchTries = 20;

        public void GenerateObjects(IAreaMap map)
        {
            var stonesCount = (int) Math.Round(map.Width * map.Height * StonesCountMultiplier);
            AddStones(map, stonesCount);
        }

        private void AddStones(IAreaMap map, int stonesCount)
        {
            for (int counter = 0; counter < stonesCount; counter++)
            {
                var position = GetFreePosition(map);
                if (position == null)
                    continue;

                map.AddObject(position, Injector.Current.Create<IDecorativeObject>(new DecorativeObjectConfiguration
                {
                    Name = "Stones",
                    Size = ObjectSize.Small,
                    Type = DecorativeObjectConfiguration.ObjectType.StonesSmall
                }));
            }
        }

        private Point GetFreePosition(IAreaMap map)
        {
            for (int counter = 0; counter < MaxPositionSearchTries; counter++)
            {
                var randomX = RandomHelper.GetRandomValue(0, map.Width - 1);
                var randomY = RandomHelper.GetRandomValue(0, map.Height - 1);

                var position = new Point(randomX, randomY);
                var cell = map.GetCell(position);
                var wall = cell.Objects.OfType<WallObject>().FirstOrDefault();
                if (wall == null)
                {
                    return position;
                }
            }
            return null;
        }
    }
}