using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Objects
{
    public interface IUsableObject
    {
        void Use(CurrentGame.GameCore<Player> game, Point position);
    }
}