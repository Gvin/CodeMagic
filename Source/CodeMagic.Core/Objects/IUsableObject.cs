using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects
{
    public interface IUsableObject
    {
        void Use(IGameCore game, Point position);
    }
}