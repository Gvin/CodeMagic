using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    internal static class MapLightLevelHelper
    {
        public static void ResetLightLevel(IAreaMap map)
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var position = new Point(x, y);
                    var cell = map.GetCell(position);
                    cell.ResetLightLevel();
                }
            }
        }

        public static void UpdateLightLevel(IAreaMap map)
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var position = new Point(x, y);
                    var cell = map.GetCell(position);
                    UpdateCellLightLevel(map, cell, position);
                }
            }
        }

        private static void UpdateCellLightLevel(IAreaMap map, AreaMapCell cell, Point position)
        {
            var lightSources = cell.Objects.OfType<ILightSource>().Where(source => source.IsLightOn).ToArray();
            if (lightSources.Length == 0)
                return;

            var maxLightLevel = lightSources.Max(source => source.LightPower);
            if (cell.LightLevel < maxLightLevel)
            {
                cell.LightLevel = maxLightLevel;
            }

            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Up), maxLightLevel);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Down), maxLightLevel);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Left), maxLightLevel);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Right), maxLightLevel);
        }

        private static void SpreadLightLevel(IAreaMap map, Point position, LightLevel lightLevel)
        {
            if (lightLevel == LightLevel.Darkness)
                return;

            if (!map.ContainsCell(position))
                return;

            var cell = map.GetCell(position);
            if (cell.LightLevel >= lightLevel)
                return;

            cell.LightLevel = lightLevel;

            if (cell.BlocksEnvironment)
                return;

            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Up), cell.LightLevel - 1);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Down), cell.LightLevel - 1);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Left), cell.LightLevel - 1);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Right), cell.LightLevel - 1);
        }
    }
}