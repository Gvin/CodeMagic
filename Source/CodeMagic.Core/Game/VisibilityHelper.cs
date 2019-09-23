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
            return new AreaMapFragment(areaOfVisibility, visibilityDiameter, visibilityDiameter);
        }

        private static void ApplyVisibilityBlockers(IAreaMapCell[][] visibleArea, Point[] visibilityBlockers)
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

        private static Point[] GetVisibilityBlockers(IAreaMapCell[][] visibleArea)
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