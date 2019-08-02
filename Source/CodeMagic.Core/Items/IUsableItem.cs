using CodeMagic.Core.Game;

namespace CodeMagic.Core.Items
{
    public interface IUsableItem
    {
        bool Use(IGameCore game);
    }
}