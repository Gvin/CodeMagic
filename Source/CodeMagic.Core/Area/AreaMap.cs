using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public class AreaMap : IAreaMap
    {
        private readonly Dictionary<Type, Point> objectPositionCache;
        private readonly AreaMapCell[][] cells;
        private readonly Dictionary<string, IDestroyableObject> destroyableObjects;
        private readonly IEnvironmentLightManager environmentLightManager;

        public AreaMap(Func<IEnvironment> environmentFactory, int width, int height, IEnvironmentLightManager environmentLightManager, int lightSpreadFactor = 1)
        {
            objectPositionCache = new Dictionary<Type, Point>();
            this.environmentLightManager = environmentLightManager;
            destroyableObjects = new Dictionary<string, IDestroyableObject>();

            LightDropFactor = lightSpreadFactor;

            Width = width;
            Height = height;

            cells = new AreaMapCell[height][];
            for (var y = 0; y < height; y++)
            {
                cells[y] = new AreaMapCell[width];
                for (var x = 0; x < width; x++)
                {
                    cells[y][x] = new AreaMapCell(environmentFactory());
                }
            }
        }

        public int LightDropFactor { get; }

        public LightLevel BackgroundLight { get; private set; }

        public IDestroyableObject GetDestroyableObject(string id)
        {
            if (destroyableObjects.ContainsKey(id))
                return destroyableObjects[id];

            return null;
        }

        public int Width { get; }

        public int Height { get; }

        public IAreaMapCell GetCell(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Coordinate X value is {x} which doesn't match map size {Width}");
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Coordinate Y value is {y} which doesn't match map size {Height}");

            return cells[y][x];
        }

        public IAreaMapCell TryGetCell(Point position)
        {
            return TryGetCell(position.X, position.Y);
        }

        public IAreaMapCell TryGetCell(int x, int y)
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

            GetOriginalCell(position.X, position.Y).ObjectsCollection.Add(@object);

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

            GetOriginalCell(position.X, position.Y).ObjectsCollection.Remove(@object);
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
                    if (cell.ObjectsCollection.Any(selector))
                        return new Point(x, y);
                }
            }

            return null;
        }

        private AreaMapCell GetOriginalCell(int x, int y)
        {
            return (AreaMapCell) GetCell(x, y);
        }

        public IAreaMapCell GetCell(Point point)
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

        public IAreaMapCell[][] GetMapPart(Point position, int radius)
        {
            var startIndexX = position.X - radius;
            var startIndexY = position.Y - radius;
            var visionDiameter = radius * 2 + 1;

            var result = new IAreaMapCell[visionDiameter][];
            for (var y = 0; y < visionDiameter; y++)
            {
                result[y] = new IAreaMapCell[visionDiameter];
                for (var x = 0; x < visionDiameter; x++)
                {
                    result[y][x] = TryGetCell(startIndexX + x, startIndexY + y);
                }
            }

            return result;
        }

        public void Refresh(DateTime gameTime)
        {
            BackgroundLight = environmentLightManager.GetEnvironmentLight(gameTime);
            MapLightLevelHelper.ResetLightLevel(this);
            MapLightLevelHelper.UpdateLightLevel(this);
        }

        public void PreUpdate(IJournal journal)
        {
            PreUpdateCells();
        }

        public void Update(IJournal journal, DateTime gameTime)
        {
            BackgroundLight = environmentLightManager.GetEnvironmentLight(gameTime);

            objectPositionCache.Clear();

            UpdateCells(journal, UpdateOrder.Early);

            MapLightLevelHelper.ResetLightLevel(this);
            MapLightLevelHelper.UpdateLightLevel(this);

            UpdateCells(journal, UpdateOrder.Medium);
            UpdateCells(journal, UpdateOrder.Late);

            PostUpdateCells(journal);
        }

        private void UpdateCells(IJournal journal, UpdateOrder order)
        {
            for (var y = 0; y < cells.Length; y++)
            {
                var row = cells[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var cell = row[x];
                    cell.Update(this, journal, new Point(x, y), order);
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
                    var cellDestroyableObjects = cell.ObjectsCollection.OfType<IDestroyableObject>();
                    foreach (var destroyableObject in cellDestroyableObjects)
                    {
                        destroyableObject.ClearDamageRecords();
                    }
                }
            }
        }

        private void PostUpdateCells(IJournal journal)
        {
            var mergedCells = new CellPairsStorage(Width, Height);
            for (var y = 0; y < cells.Length; y++)
            {
                for (var x = 0; x < cells[y].Length; x++)
                {
                    var position = new Point(x, y);
                    var cell = (AreaMapCell)GetCell(position);
                    cell.PostUpdate(this, journal, position);
                    cell.ResetDynamicObjectsState();
                    cell.Environment.Update(this, position, cell, journal);
                    MergeCellEnvironment(position, cell, mergedCells);
                }
            }
        }

        private void MergeCellEnvironment(Point position, AreaMapCell cell, CellPairsStorage mergedCells)
        {
            if (cell.BlocksEnvironment)
                return;
            SpreadEnvironment(position, cell, mergedCells);
        }

        private void SpreadEnvironment(Point position, AreaMapCell cell, CellPairsStorage mergedCells)
        {
            TrySpreadEnvironment(position, Direction.North, cell, mergedCells);
            TrySpreadEnvironment(position, Direction.South, cell, mergedCells);
            TrySpreadEnvironment(position, Direction.West, cell, mergedCells);
            TrySpreadEnvironment(position, Direction.East, cell, mergedCells);
        }

        private void TrySpreadEnvironment(Point position, Direction direction, AreaMapCell cell, CellPairsStorage mergedCells)
        {
            var nextPosition = Point.GetPointInDirection(position, direction);
            if (!ContainsCell(nextPosition))
                return;

            var nextCell = (AreaMapCell) GetCell(nextPosition);

            if (mergedCells.ContainsPair(position, direction))
                return;
            mergedCells.RegisterPair(position, direction);

            if (nextCell.BlocksEnvironment)
                return;

            cell.CheckSpreading(nextCell);
            cell.Environment.Balance(cell, nextCell);
        }

        private class CellPairsStorage
        {
            private readonly List<Direction>[,] pairs;

            public CellPairsStorage(int width, int height)
            {
                pairs = new List<Direction>[width, height];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        pairs[x, y] = new List<Direction>();
                    }
                }
            }

            public void RegisterPair(Point initialCell, Direction direction)
            {
                pairs[initialCell.X, initialCell.Y].Add(direction);
                var targetCell = Point.GetPointInDirection(initialCell, direction);
                var invertedDirection = DirectionHelper.InvertDirection(direction);
                pairs[targetCell.X, targetCell.Y].Add(invertedDirection);
            }

            public bool ContainsPair(Point initialCell, Direction direction)
            {
                return pairs[initialCell.X, initialCell.Y].Contains(direction);
            }
        }
    }
}