using CodeMagic.Core.Game;

namespace CodeMagic.Core.Area
{
    public interface IEnvironment
    {
        void Update(Point position, IAreaMapCell cell);

        void Balance(IAreaMapCell cell, IAreaMapCell otherCell);
    }
}