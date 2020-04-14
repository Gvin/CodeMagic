using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;

namespace CodeMagic.Game.PlayerActions
{
    public class EmptyPlayerAction : IPlayerAction
    {
        public bool Perform(out Point newPosition)
        {
            // Do nothing.
            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}