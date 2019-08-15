using System;
using System.ComponentModel;
using System.Diagnostics;
using CodeMagic.Core.Common;

namespace CodeMagic.Core.Game
{
    [DebuggerDisplay("[{X}:{Y}]")]
    public class Point
    {
        public Point()
        {
            X = 0;
            Y = 0;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public bool Equals(Point other)
        {
            return other != null && X == other.X && Y == other.Y;
        }

        public static bool IsAdjustedPoint(Point point1, Point point2)
        {
            return point1.X == point2.X && Math.Abs(point1.Y - point2.Y) == 1 ||
                   point1.Y == point2.Y && Math.Abs(point1.X - point2.X) == 1;
        }

        public static Direction? GetAdjustedPointRelativeDirection(Point position, Point checkPoint)
        {
            if (position.X == checkPoint.X && checkPoint.Y == position.Y - 1)
                return Direction.North;
            if (position.X == checkPoint.X && checkPoint.Y == position.Y + 1)
                return Direction.South;
            if (position.Y == checkPoint.Y && checkPoint.X == position.X - 1)
                return Direction.West;
            if (position.Y == checkPoint.Y && checkPoint.X == position.X + 1)
                return Direction.East;

            return null;
        }

        public static Point GetPointInDirection(Point point, Direction direction, int distance = 1)
        {
            switch (direction)
            {
                case Direction.North:
                    return new Point(point.X, point.Y - distance);
                case Direction.South:
                    return new Point(point.X, point.Y + distance);
                case Direction.West:
                    return new Point(point.X - distance, point.Y);
                case Direction.East:
                    return new Point(point.X + distance, point.Y);
                default:
                    throw new InvalidEnumArgumentException($"Unknown direction: {direction}");
            }
        }

        public static bool IsOnLine(Point startPoint, Point endPoint, Point point, double tolerance = 0.3d)
        {
            var ab = Math.Sqrt((endPoint.X - startPoint.X) * (endPoint.X - startPoint.X) + (endPoint.Y - startPoint.Y) * (endPoint.Y - startPoint.Y));
            var ap = Math.Sqrt((point.X - startPoint.X) * (point.X - startPoint.X) + (point.Y - startPoint.Y) * (point.Y - startPoint.Y));
            var pb = Math.Sqrt((endPoint.X - point.X) * (endPoint.X - point.X) + (endPoint.Y - point.Y) * (endPoint.Y - point.Y));
            return Math.Abs(ab - (ap + pb)) < tolerance;
        }

        public static double GetDistance(Point point1, Point point2)
        {
            return Math.Sqrt(
                Math.Pow(point2.X - point1.X, 2) +
                Math.Pow(point2.Y - point1.Y, 2));
        }
    }
}