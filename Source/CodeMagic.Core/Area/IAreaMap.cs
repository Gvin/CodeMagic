using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public interface IAreaMap
    {
        int Width { get; }

        int Height { get; }

        /// <summary>
        /// Refreshes map before the first rendering.
        /// Calculates light level and etc.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Gets cell with specified coordinates.
        /// Throws exception if coordinates are not on the map.
        /// </summary>
        AreaMapCell GetCell(int x, int y);

        /// <summary>
        /// Gets cell with specified position.
        /// Throws exception if position is not on the map.
        /// </summary>
        AreaMapCell GetCell(Point point);

        /// <summary>
        /// Gets cell with specified position.
        /// Returns null if position is not on the map.
        /// </summary>
        AreaMapCell TryGetCell(Point point);

        /// <summary>
        /// Gets cell with specified coordinates.
        /// Returns null if coordinates are not on the map.
        /// </summary>
        AreaMapCell TryGetCell(int x, int y);

        /// <summary>
        /// Gets if map contains cell with specified coordinates.
        /// </summary>
        bool ContainsCell(int x, int y);

        /// <summary>
        /// Gets if map contains cell with specified position.
        /// </summary>
        bool ContainsCell(Point point);

        /// <summary>
        /// Resets damage records for all destroyable objects.
        /// Should be called before player action.
        /// </summary>
        void PreUpdate(IGameCore game);

        /// <summary>
        /// Updates entire map.
        /// Should be called after player action.
        /// </summary>
        void Update(IGameCore game);

        IDestroyableObject GetDestroyableObject(string id);

        void AddObject(Point position, IMapObject @object);

        void RemoveObject(Point position, IMapObject @object);

        AreaMapCell[][] GetMapPart(Point position, int radius);

        Point GetObjectPosition<T>() where T : IMapObject;

        Point GetObjectPosition(Func<IMapObject, bool> selector);
    }
}