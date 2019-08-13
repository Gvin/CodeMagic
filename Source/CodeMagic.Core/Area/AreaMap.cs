using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public class AreaMap : IAreaMap
    {
        private readonly Dictionary<Type, Point> objectPositionCache;
        private readonly AreaMapCell[][] cells;
        private readonly Dictionary<string, IDestroyableObject> destroyableObjects;

        public AreaMap(int width, int height)
        {
            objectPositionCache = new Dictionary<Type, Point>();

            destroyableObjects = new Dictionary<string, IDestroyableObject>();

            Width = width;
            Height = height;

            cells = new AreaMapCell[height][];
            for (var y = 0; y < height; y++)
            {
                cells[y] = new AreaMapCell[width];
                for (var x = 0; x < width; x++)
                {
                    cells[y][x] = new AreaMapCell();
                }
            }
        }

        public IDestroyableObject GetDestroyableObject(string id)
        {
            if (destroyableObjects.ContainsKey(id))
                return destroyableObjects[id];

            return null;
        }

        public int Width { get; }

        public int Height { get; }

        public AreaMapCell GetCell(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Coordinate X value is {x} which doesn't match map size {Width}");
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Coordinate Y value is {y} which doesn't match map size {Height}");

            return cells[y][x];
        }

        public AreaMapCell TryGetCell(Point position)
        {
            return TryGetCell(position.X, position.Y);
        }

        public AreaMapCell TryGetCell(int x, int y)
        {
            if (!ContainsCell(x, y))
                return null;

            return GetCell(x, y);
        }

        public void AddObject(int x, int y, IMapObject @object)
        {
            AddObject(new Point(x, y), @object);
        }

        public void AddObject(Point position, IMapObject @object)
        {
            if (@object is IDestroyableObject destroyableObject)
            {
                destroyableObjects.Add(destroyableObject.Id, destroyableObject);
            }

            GetCell(position).Objects.Add(@object);

            if (@object is IPlacedHandler placedHandler)
            {
                placedHandler.OnPlaced(this, position);
            }
        }

        public void RemoveObject(Point position, IMapObject @object)
        {
            if (@object is IDestroyableObject destroyableObject)
            {
                destroyableObjects.Remove(destroyableObject.Id);
            }

            GetCell(position).Objects.Remove(@object);
        }

        public Point GetObjectPosition<T>() where T : IMapObject
        {
            var type = typeof(T);
            if (objectPositionCache.ContainsKey(type))
                return objectPositionCache[type];

            var position = GetObjectPosition(obj => obj is T);
            objectPositionCache.Add(type, position);
            return position;
        }

        public Point GetObjectPosition(Func<IMapObject, bool> selector)
        {
            for (var y = 0; y < cells.Length; y++)
            {
                var row = cells[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var cell = row[x];
                    if (cell.Objects.Any(selector))
                        return new Point(x, y);
                }
            }

            return null;
        }

        public AreaMapCell GetCell(Point point)
        {
            return GetCell(point.X, point.Y);
        }

        public bool ContainsCell(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool ContainsCell(Point point)
        {
            return ContainsCell(point.X, point.Y);
        }

        public AreaMapCell[][] GetMapPart(Point position, int radius)
        {
            var startIndexX = position.X - radius;
            var startIndexY = position.Y - radius;
            var visionDiameter = radius * 2 + 1;

            var result = new AreaMapCell[visionDiameter][];
            for (var y = 0; y < visionDiameter; y++)
            {
                result[y] = new AreaMapCell[visionDiameter];
                for (var x = 0; x < visionDiameter; x++)
                {
                    result[y][x] = TryGetCell(startIndexX + x, startIndexY + y);
                }
            }

            return result;
        }

        public void Refresh()
        {
            MapLightLevelHelper.UpdateLightLevel(this);
        }

        public void PreUpdate(IGameCore game)
        {
            PreUpdateCells();
        }

        public void Update(IGameCore game)
        {
            objectPositionCache.Clear();

            UpdateCells(game, UpdateOrder.Early);

            MapLightLevelHelper.ResetLightLevel(this);
            MapLightLevelHelper.UpdateLightLevel(this);

            UpdateCells(game, UpdateOrder.Medium);
            UpdateCells(game, UpdateOrder.Late);

            PostUpdateCells(game);
        }

        private void UpdateCells(IGameCore game, UpdateOrder order)
        {
            for (var y = 0; y < cells.Length; y++)
            {
                var row = cells[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var cell = row[x];
                    cell.Update(game, new Point(x, y), order);
                }
            }
        }

        private void PreUpdateCells()
        {
            for (var y = 0; y < cells.Length; y++)
            {
                var row = cells[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var cell = row[x];
                    var cellDestroyableObjects = cell.Objects.OfType<IDestroyableObject>();
                    foreach (var destroyableObject in cellDestroyableObjects)
                    {
                        destroyableObject.ClearDamageRecords();
                    }
                }
            }
        }

        private void PostUpdateCells(IGameCore game)
        {
            var mergedCells = new List<CellsPair>();
            for (var y = 0; y < cells.Length; y++)
            {
                for (var x = 0; x < cells[y].Length; x++)
                {
                    var position = new Point(x, y);
                    var cell = GetCell(position);
                    cell.PostUpdate(game, position);
                    cell.ResetDynamicObjectsState();
                    cell.UpdateEnvironment();
                    MergeCellEnvironment(position, cell, mergedCells);
                }
            }
        }

        private void MergeCellEnvironment(Point position, AreaMapCell cell, List<CellsPair> mergedCells)
        {
            if (cell.BlocksEnvironment)
                return;
            SpreadEnvironment(position, cell, mergedCells);
        }

        private void SpreadEnvironment(Point position, AreaMapCell cell, List<CellsPair> mergedCells)
        {
            TrySpreadEnvironment(position, Direction.Up, cell, mergedCells);
            TrySpreadEnvironment(position, Direction.Down, cell, mergedCells);
            TrySpreadEnvironment(position, Direction.Left, cell, mergedCells);
            TrySpreadEnvironment(position, Direction.Right, cell, mergedCells);
        }

        private void TrySpreadEnvironment(Point position, Direction direction, AreaMapCell cell, List<CellsPair> mergedCells)
        {
            var nextPosition = Point.GetPointInDirection(position, direction);
            if (!ContainsCell(nextPosition))
                return;

            var nextCell = GetCell(nextPosition);
            if (mergedCells.Any(merge => merge.ContainsPair(cell, nextCell)))
                return;
            mergedCells.Add(new CellsPair(cell, nextCell));

            cell.MagicEnergy.Merge(nextCell.MagicEnergy);

            if (nextCell.BlocksEnvironment)
                return;

            cell.CheckSpreading(nextCell);
            cell.Environment.Balance(nextCell.Environment);
        }

        private class CellsPair
        {
            public CellsPair(AreaMapCell cell1, AreaMapCell cell2)
            {
                Cell1 = cell1;
                Cell2 = cell2;
            }

            private AreaMapCell Cell1 { get; }

            private AreaMapCell Cell2 { get; }

            public bool ContainsPair(AreaMapCell checkCell1, AreaMapCell checkCell2)
            {
                return (Cell1 == checkCell1 && Cell2 == checkCell2) || (Cell2 == checkCell1 && Cell1 == checkCell2);
            }
        }
    }
}