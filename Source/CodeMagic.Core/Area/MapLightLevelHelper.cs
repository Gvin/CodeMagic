using System;
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
                    cell.LightLevel = cell.HasRoof ? LightLevel.Darkness : map.BackgroundLight;
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

        private static void UpdateCellLightLevel(IAreaMap map, IAreaMapCell cell, Point position)
        {
            var lights = cell.Objects.OfType<ILightObject>()
                .SelectMany(lightObject => lightObject.LightSources)
                .Where(source => source.IsLightOn && source.LightPower > LightLevel.Darkness)
                .ToArray();

            if (lights.Length == 0)
                return;

            var maxLightPower = lights.Max(light => light.LightPower);

            cell.LightLevel = (LightLevel)Math.Max((int)cell.LightLevel, (int)maxLightPower);

            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.North), maxLightPower);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.South), maxLightPower);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.West), maxLightPower);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.East), maxLightPower);
        }

        private static void SpreadLightLevel(IAreaMap map, Point position, LightLevel light)
        {
            if (light <= LightLevel.Darkness)
                return;

            var cell = map.TryGetCell(position);
            if (cell == null)
                return;

            if ((int) light < (int) cell.LightLevel)
                return;
            cell.LightLevel = light;

            if (cell.BlocksEnvironment)
                return;

            var lightPowerToSpread = (int) light - 1;
            if (lightPowerToSpread <= (int)LightLevel.Darkness)
                return;

            var lightToSpread = (LightLevel) lightPowerToSpread;
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.North), lightToSpread);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.South), lightToSpread);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.West), lightToSpread);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.East), lightToSpread);
        }
    }
}