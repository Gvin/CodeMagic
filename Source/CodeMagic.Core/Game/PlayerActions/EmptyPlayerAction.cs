using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game.PlayerActions
{
    public class EmptyPlayerAction : IPlayerAction
    {
        public bool Perform(IPlayer player, Point playerPosition, IGameCore game, out Point newPosition)
        {
            // Do nothing.
            newPosition = playerPosition;
            return true;
        }
    }
}