using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;

namespace CodeMagic.Core.Game
{
    public static class VisibilityHelper
    {
        public static AreaMapFragment GetVisibleArea(int visibilityRange, Point viewerPosition, IAreaMap map)
        {
            var areaOfVisibility = map.GetMapPart(viewerPosition, visibilityRange);
            var visibilityBlockers = GetVisibilityBlockers(areaOfVisibility);
            ApplyVisibilityBlockers(areaOfVisibility, visibilityBlockers);

            var visibilityDiameter = visibilityRange * 2 + 1;
            var visibleAreaPosition = new Point(viewerPosition.X - visibilityRange, viewerPosition.Y - visibilityRange);
            return new AreaMapFragment(areaOfVisibility, visibilityDiameter, visibilityDiameter, visibleAreaPosition);
        }

        private static void ApplyVisibilityBlockers(AreaMapCell[][] visibleArea, Point[] visibilityBlockers)
        {
            var center = (visibleArea.Length - 1) / 2;
            var viewerPosition = new Point(center, center);
            for (var y = 0; y < visibleArea.Length; y++)
            {
                var row = visibleArea[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var cellPosition = new Point(x, y);
                    if (!cellPosition.Equals(viewerPosition) && GetIfVisibilityBlocked(cellPosition, visibilityBlockers, viewerPosition))
                    {
                        row[x] = null;
                    }
                }
            }
        }

        private static bool GetIfVisibilityBlocked(Point position, Point[] visibilityBlockers, Point objectPosition)
        {
            return visibilityBlockers.Any(blocker =>
                !position.Equals(blocker) && 
                Point.IsOnLine(objectPosition, position, blocker));
        }

        private static Point[] GetVisibilityBlockers(AreaMapCell[][] visibleArea)
        {
            var result = new List<Point>();
            for (var y = 0; y < visibleArea.Length; y++)
            {
                var row = visibleArea[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var cell = row[x];
                    if (cell != null && cell.BlocksVisibility)
                    {
                        result.Add(new Point(x, y));
                    }
                }
            }

            return result.ToArray();
        }
    }
}