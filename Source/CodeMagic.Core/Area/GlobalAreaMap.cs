using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public class GlobalAreaMap : IAreaMap
    {
        private readonly Dictionary<Type, Point> objectPositionCache;
        private readonly GlobalAreaMapCell[][] cells;
        private readonly Dictionary<string, IDestroyableObject> destroyableObjects;
        private readonly IEnvironmentLightManager environmentLightManager;

        public GlobalAreaMap(IPhysicsConfiguration configuration, int width, int height, IEnvironmentLightManager environmentLightManager, int lightSpreadFactor = 1)
        {
            objectPositionCache = new Dictionary<Type, Point>();
            this.environmentLightManager = environmentLightManager;
            destroyableObjects = new Dictionary<string, IDestroyableObject>();

            LightDropFactor = lightSpreadFactor;

            Width = width;
            Height = height;

            cells = new GlobalAreaMapCell[height][];
            for (var y = 0; y < height; y++)
            {
                cells[y] = new GlobalAreaMapCell[width];
                for (var x = 0; x < width; x++)
                {
                    cells[y][x] = new GlobalAreaMapCell(configuration);
                }
            }
        }

        public int Width { get; }

        public int Height { get; }

        public int LightDropFactor { get; }

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

        private GlobalAreaMapCell GetOriginalCell(int x, int y)
        {
            return (GlobalAreaMapCell)GetCell(x, y);
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
            // Do nothing
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

        private void PostUpdateCells(IJournal journal)
        {
            for (var y = 0; y < cells.Length; y++)
            {
                for (var x = 0; x < cells[y].Length; x++)
                {
                    var position = new Point(x, y);
                    var cell = (GlobalAreaMapCell)GetCell(position);
                    cell.PostUpdate(this, journal, position);
                    cell.ResetDynamicObjectsState();
                }
            }
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

        public IDestroyableObject GetDestroyableObject(string id)
        {
            if (destroyableObjects.ContainsKey(id))
                return destroyableObjects[id];

            return null;
        }

        public LightLevel BackgroundLight { get; private set; }
    }
}