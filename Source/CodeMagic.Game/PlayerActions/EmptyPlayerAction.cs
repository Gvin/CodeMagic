using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class EmptyPlayerAction : PlayerActionBase
    {
        protected override int RestoresStamina => 20;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
        {
            // Do nothing.
            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}