using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public interface IAreaMap
    {
        int Level { get; }

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
        IAreaMapCell GetCell(int x, int y);

        /// <summary>
        /// Gets cell with specified position.
        /// Throws exception if position is not on the map.
        /// </summary>
        IAreaMapCell GetCell(Point point);

        /// <summary>
        /// Gets cell with specified position.
        /// Returns null if position is not on the map.
        /// </summary>
        IAreaMapCell TryGetCell(Point point);

        /// <summary>
        /// Gets cell with specified coordinates.
        /// Returns null if coordinates are not on the map.
        /// </summary>
        IAreaMapCell TryGetCell(int x, int y);

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
        void PreUpdate(IJournal journal);

        /// <summary>
        /// Updates entire map.
        /// Should be called after player action.
        /// </summary>
        void Update(IJournal journal);

        IDestroyableObject GetDestroyableObject(string id);

        void AddObject(Point position, IMapObject @object);

        void RemoveObject(Point position, IMapObject @object);

        IAreaMapCell[][] GetMapPart(Point position, int radius);

        Point GetObjectPosition<T>() where T : IMapObject;

        Point GetObjectPosition(Func<IMapObject, bool> selector);
    }
}