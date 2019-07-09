using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public interface IAreaMap
    {
        int Width { get; }

        int Height { get; }

        AreaMapCell GetCell(int x, int y);

        AreaMapCell GetCell(Point point);

        bool ContainsCell(int x, int y);

        bool ContainsCell(Point point);

        void Update(Journal journal);

        void RegisterDestroyableObject(IDestroyableObject @object);

        void UnregisterDestroyableObject(IDestroyableObject @object);

        IDestroyableObject GetDestroyableObject(string id);

        void AddObject(Point position, IMapObject @object);

        AreaMapCell[][] GetMapPart(Point position, int radius);

        Point GetObjectPosition<T>() where T : IMapObject;
    }
}