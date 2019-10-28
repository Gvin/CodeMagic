using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Core.Items
{
    public interface IUsableItem : IItem
    {
        bool Use(GameCore<Player> game);
    }
}