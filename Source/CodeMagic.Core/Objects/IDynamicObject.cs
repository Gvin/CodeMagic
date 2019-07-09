using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Objects
{
    public interface IDynamicObject
    {
        void Update(IAreaMap map, Point position, Journal journal);

        bool Updated { get; set; }
    }
}