using CodeMagic.Core.Game;

namespace CodeMagic.Core.Items
{
    public interface IUsableItem : IItem
    {
        bool Use(IGameCore game);
    }
}