using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public class SolidObject : IMapObject, IPlacedHandler
    {
        private readonly List<Direction> connectedTiles;

        public SolidObject(SolidObjectConfiguration configuration)
        {
            Name = configuration.Name;
            Type = configuration.Type;
            connectedTiles = new List<Direction>();
        }

        public string Name { get; }

        public string Type { get; }

        public bool BlocksMovement => true;

        public bool IsVisible => true;

        public bool BlocksVisibility => true;

        public bool BlocksProjectiles => true;

        public bool BlocksEnvironment => true;

        public ZIndex ZIndex => ZIndex.Wall;

        public bool HasConnectedTile(Direction direction)
        {
            return connectedTiles.Contains(direction);
        }

        public void RegisterConnectedTile(Direction direction)
        {
            if (!connectedTiles.Contains(direction))
                connectedTiles.Add(direction);
        }

        public void OnPlaced(IAreaMap map, Point position)
        {
            CheckWallInDirection(map, position, Direction.Up);
            CheckWallInDirection(map, position, Direction.Down);
            CheckWallInDirection(map, position, Direction.Left);
            CheckWallInDirection(map, position, Direction.Right);
        }

        private void CheckWallInDirection(IAreaMap map, Point position, Direction direction)
        {
            var wallUp = GetWallNear(map, position, direction);
            if (wallUp != null)
            {
                connectedTiles.Add(direction);
                wallUp.RegisterConnectedTile(NegateDirection(direction));
            }
        }

        private Direction NegateDirection(Direction direction)
        {
            return (Direction) ((int)direction * -1);
        }

        private SolidObject GetWallNear(IAreaMap map, Point position, Direction direction)
        {
            var nearPosition = Point.GetPointInDirection(position, direction);
            if (!map.ContainsCell(nearPosition))
                return null;

            var cell = map.GetCell(nearPosition);
            return cell.Objects.OfType<SolidObject>().FirstOrDefault();
        }
    }

    public class SolidObjectConfiguration
    {
        public const string ObjectTypeWallWood = "WallWood";
        public const string ObjectTypeWallStone = "WallStone";
        public const string ObjectTypeHole = "Hole";

        public string Name { get; set; }

        public string Type { get; set; }
    }
}