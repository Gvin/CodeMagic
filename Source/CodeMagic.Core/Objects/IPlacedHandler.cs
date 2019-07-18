using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects
{
    public interface IPlacedHandler
    {
        void OnPlaced(IAreaMap map, Point position);
    }
}