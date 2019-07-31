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
            var lights = cell.Objects.OfType<ILightSource>().Where(source => source.IsLightOn)
                .Select(source => new Light(source)).ToArray();
            if (lights.Length == 0)
                return;

            cell.LightLevel.AddLights(lights);

            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Up), lights);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Down), lights);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Left), lights);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Right), lights);
        }

        private static void SpreadLightLevel(IAreaMap map, Point position, Light[] lights)
        {
            if (lights.Length == 0 || lights.All(light => light.Power == LightLevel.Darkness))
                return;

            var cell = map.TryGetCell(position);
            if (cell == null)
                return;

            cell.LightLevel.AddLights(lights);

            if (cell.BlocksEnvironment)
                return;

            var lightsToSpread = lights.Select(light => light.Clone(light.Power - 1)).Where(light => light.Power > LightLevel.Darkness).ToArray();

            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Up), lightsToSpread);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Down), lightsToSpread);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Left), lightsToSpread);
            SpreadLightLevel(map, Point.GetPointInDirection(position, Direction.Right), lightsToSpread);
        }
    }
}