using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public interface IAreaMapCellInternal : IAreaMapCell
    {
        MapObjectsCollection ObjectsCollection { get; }

        void Update(Point position, UpdateOrder updateOrder);

        void PostUpdate(IAreaMap map, Point position);

        void ResetDynamicObjectsState();

        void CheckSpreading(IAreaMapCellInternal other);
    }
}