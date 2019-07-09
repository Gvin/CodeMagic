using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects.SolidObjects;

namespace CodeMagic.Core.Game
{
    public class VisibilityManager
    {
        public VisibleArea GetVisibleArea(int visibilityRange, Point viewerPosition, IAreaMap map)
        {
            var areaOfVisibility = map.GetMapPart(viewerPosition, visibilityRange);
            var visibilityBlockers = GetVisibilityBlockers(areaOfVisibility);
            ApplyVisibilityBlockers(areaOfVisibility, visibilityBlockers);
            return new VisibleArea(areaOfVisibility);
        }

        private void ApplyVisibilityBlockers(AreaMapCell[][] visibleArea, Point[] visibilityBlockers)
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

        private bool GetIfVisibilityBlocked(Point position, Point[] visibilityBlockers, Point objectPosition)
        {
            return visibilityBlockers.Any(blocker =>
                !position.Equals(blocker) && 
                Point.IsOnLine(objectPosition, position, blocker));
        }

        private Point[] GetVisibilityBlockers(AreaMapCell[][] visibleArea)
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

        private AreaMapCell[][] GetCellsInVisibilityRange(int visibilityRange, Point playerPosition, IAreaMap map)
        {
            var result = map.GetMapPart(playerPosition, visibilityRange);
            foreach (var row in result)
            {
                for (var x = 0; x < row.Length; x++)
                {
                    if (row[x] == null)
                    {
                        row[x] = new AreaMapCell {FloorType = FloorTypes.Hole};
                    }
                    else
                    {
                        row[x] = row[x].Clone();
                    }
                }
            }

            return result;
        }

        private void MarkCellAsInvisible(AreaMapCell cell)
        {
            cell.Objects.Clear();
            cell.Objects.Add(new SolidObject(new SolidObjectConfiguration
            {
                Name = "Unknown",
                Type = SolidObjectConfiguration.ObjectTypeHole
            }));
        }
    }
}