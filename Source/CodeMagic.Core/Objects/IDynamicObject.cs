using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects
{
    public interface IDynamicObject
    {
        void Update(Point position);

        bool Updated { get; set; }

        UpdateOrder UpdateOrder { get; }
    }
}