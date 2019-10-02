using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects
{
    public interface IPlaceConnectionObject : IPlacedHandler, IMapObject
    {
        void AddConnectedTile(Point position);
    }
}