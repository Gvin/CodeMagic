using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Area
{
    public interface IEnvironment
    {
        void Update(IAreaMap map, Point position, IAreaMapCell cell, IJournal journal);

        void Balance(IAreaMapCell cell, IAreaMapCell otherCell);
    }
}