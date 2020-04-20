using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public abstract class WallBase : MapObjectBase, IPlaceConnectionObject
    {
        private const string SaveKeyConnectedTiles = "ConnectedTiles";

        private readonly List<Point> connectedTiles;

        protected WallBase(SaveData data) : base(data)
        {
            connectedTiles = data.GetObjectsCollection<Point>(SaveKeyConnectedTiles).ToList();
        }

        protected WallBase(string name)
            : base(name)
        {
            connectedTiles = new List<Point>();
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyConnectedTiles, connectedTiles);
            return data;
        }

        public override ObjectSize Size => ObjectSize.Huge;

        public void AddConnectedTile(Point position)
        {
            connectedTiles.Add(position);
        }

        public override bool BlocksMovement => true;

        public override bool BlocksAttack => true;

        public override bool BlocksVisibility => true;

        public override bool BlocksProjectiles => true;

        public override bool BlocksEnvironment => true;

        public override ZIndex ZIndex => ZIndex.Wall;

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

        public abstract bool CanConnectTo(IMapObject mapObject);

        private IPlaceConnectionObject GetWall(IAreaMap map, Point position, int relativeX, int relativeY)
        {
            var nearPosition = new Point(position.X + relativeX, position.Y + relativeY);
            var cell = map.TryGetCell(nearPosition);
            return cell?.Objects.OfType<IPlaceConnectionObject>().FirstOrDefault(obj => CanConnectTo(obj) || obj.CanConnectTo(this));
        }
    }
}