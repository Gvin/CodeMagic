using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public abstract class WallBase : IPlaceConnectionObject
    {
        private readonly List<Point> connectedTiles;

        protected WallBase()
        {
            connectedTiles = new List<Point>();
        }

        public ObjectSize Size => ObjectSize.Huge;

        public void AddConnectedTile(Point position)
        {
            connectedTiles.Add(position);
        }

        public abstract string Name { get; }

        public virtual bool BlocksMovement => true;

        public virtual bool BlocksAttack => true;

        public bool IsVisible => true;

        public virtual bool BlocksVisibility => true;

        public virtual bool BlocksProjectiles => true;

        public virtual bool BlocksEnvironment => true;

        public ZIndex ZIndex => ZIndex.Wall;

        protected bool HasConnectedTile(int relativeX, int relativeY)
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
                wallUp.AddConnectedTile(new Point(relativeX* (-1), relativeY * (-1)));
            }
        }

        protected abstract bool CanConnectTo(IMapObject mapObject);

        private IPlaceConnectionObject GetWall(IAreaMap map, Point position, int relativeX, int relativeY)
        {
            var nearPosition = new Point(position.X + relativeX, position.Y + relativeY);
            var cell = map.TryGetCell(nearPosition);
            return cell?.Objects.OfType<IPlaceConnectionObject>().FirstOrDefault(CanConnectTo);
        }

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }
    }
}