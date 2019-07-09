using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.CreaturesLogic
{
    public static class CreaturesVisibilityHelper
    {
        public static bool GetIfPointIsVisible(IAreaMap map, Point position, int viewDistance, Point checkPoint)
        {
            if (!GetIfPointInVisibilityRange(position, viewDistance, checkPoint))
                return false;

            var visibleArea = VisibilityHelper.GetVisibleArea(viewDistance, position, map);
            var relativeX = checkPoint.X - position.X + viewDistance;
            var relativeY = checkPoint.Y - position.Y + viewDistance;

            var checkCell = visibleArea.GetCell(relativeX, relativeY);
            if (checkCell == null)
                return false;

            return true;
        }

        private static bool GetIfPointInVisibilityRange(Point position, int viewDistance, Point checkPoint)
        {
            var startX = position.X - viewDistance;
            var startY = position.Y - viewDistance;
            var viewDiameter = viewDistance * 2;
            var endX = startX + viewDiameter;
            var endY = startY + viewDiameter;
            return checkPoint.X >= startX && checkPoint.X <= endX && checkPoint.Y >= startY && checkPoint.Y <= endY;
        }
    }
}