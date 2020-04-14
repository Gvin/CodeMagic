using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public interface IUsableItem : IItem
    {
        bool Use(CurrentGame.GameCore<Player> game);
    }
}