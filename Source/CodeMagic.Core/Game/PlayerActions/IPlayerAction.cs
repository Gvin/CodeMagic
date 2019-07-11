using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game.PlayerActions
{
    public interface IPlayerAction
    {
        bool Perform(IPlayer player, Point playerPosition, IGameCore game, out Point newPosition);
    }
}