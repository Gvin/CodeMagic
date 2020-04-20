using CodeMagic.Core.Game;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Area
{
    public interface IEnvironment : ISaveable
    {
        void Update(Point position, IAreaMapCell cell);

        void Balance(IAreaMapCell cell, IAreaMapCell otherCell);
    }
}