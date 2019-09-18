using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Objects
{
    public interface IDynamicObject
    {
        void Update(IAreaMap map, IJournal journal, Point position);

        bool Updated { get; set; }

        UpdateOrder UpdateOrder { get; }
    }
}