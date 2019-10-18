using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;

namespace CodeMagic.Game.PlayerActions
{
    public class EmptyPlayerAction : IPlayerAction
    {
        public bool Perform(IGameCore game, out Point newPosition)
        {
            // Do nothing.
            newPosition = game.PlayerPosition;
            return true;
        }
    }
}