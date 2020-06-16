using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects
{
    public interface IDynamicObject : IMapObject
    {
        void Update(Point position);

        bool Updated { get; set; }

        UpdateOrder UpdateOrder { get; }
    }
}