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
                    cell.LightLevel.Clear();

                    if (map.BackgroundLight != null)
                    {
                        cell.LightLevel.AddLight(map.BackgroundLight.Clone());
                    }
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
                .Select(source => new Light(source))
                .ToArray();

            if (lights.Length == 0)
                return;

            cell.LightLevel.AddLights(lights);

            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.North), lights);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.South), lights);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.West), lights);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.East), lights);
        }

        private static void SpreadLightLevel(IAreaMap map, Point position, Light[] lights)
        {
            if (lights.Length == 0 || lights.All(light => light.Power <= LightLevel.Darkness))
                return;

            var cell = map.TryGetCell(position);
            if (cell == null)
                return;

            cell.LightLevel.AddLights(lights);

            if (cell.BlocksEnvironment)
                return;

            var lightsToSpread = lights
                .Select(light => light.Clone(light.Power - map.LightDropFactor))
                .Where(light => light.Power > LightLevel.Darkness)
                .ToArray();

            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.North), lightsToSpread);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.South), lightsToSpread);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.West), lightsToSpread);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.East), lightsToSpread);
        }
    }
}