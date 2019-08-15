using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public class WallObject : IMapObject, IPlacedHandler
    {
        private readonly List<Point> connectedTiles;

        public WallObject(WallObjectConfiguration configuration)
        {
            Name = configuration.Name;
            Type = configuration.Type;
            connectedTiles = new List<Point>();
        }

        public ObjectSize Size => ObjectSize.Huge;

        public string Name { get; }

        public WallObjectConfiguration.WallType Type { get; }

        public bool BlocksMovement => true;

        public bool IsVisible => true;

        public bool BlocksVisibility => true;

        public bool BlocksProjectiles => true;

        public bool BlocksEnvironment => true;

        public ZIndex ZIndex => ZIndex.Wall;

        public bool HasConnectedTile(int relativeX, int relativeY)
        {
            return connectedTiles.Any(point => point.X == relativeX && point.Y == relativeY);
        }

        public void OnPlaced(IAreaMap map, Point position)
        {
            CheckWallInDirection(map, position, -1, -1); // Top Left
            CheckWallInDirection(map, position, 0, -1); // Top
            CheckWallInDirection(map, position, 1, -1); // Top Right

            CheckWallInDirection(map, position, -1, 0); // Left
            CheckWallInDirection(map, position, 1, 0); // Right

            CheckWallInDirection(map, position, 1, 1); // Bottom Left
            CheckWallInDirection(map, position, 0, 1); // Bottom
            CheckWallInDirection(map, position, 1, 1); // Bottom Right
        }

        private void CheckWallInDirection(IAreaMap map, Point position, int relativeX, int relativeY)
        {
            var wallUp = GetWall(map, position, relativeX, relativeY);
            if (wallUp != null)
            {
                connectedTiles.Add(new Point(relativeX, relativeY));
                wallUp.connectedTiles.Add(new Point(relativeX* (-1), relativeY * (-1)));
            }
        }

        private WallObject GetWall(IAreaMap map, Point position, int relativeX, int relativeY)
        {
            var nearPosition = new Point(position.X + relativeX, position.Y + relativeY);
            var cell = map.TryGetCell(nearPosition);
            return cell?.Objects.OfType<WallObject>().FirstOrDefault();
        }

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }
    }

    public class WallObjectConfiguration
    {
        public enum WallType
        {
            Wood,
            Stone,
            Hole
        }

        public string Name { get; set; }

        public WallType Type { get; set; }
    }
}