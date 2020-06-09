using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public abstract class PlayerActionBase : IPlayerAction
    {
        public bool Perform(out Point newPosition)
        {
            var game = (GameCore<Player>)CurrentGame.Game;
            var result = Perform(game, out newPosition);

            if (result)
            {
                game.Player.Stamina += RestoresStamina;
            }
            return result;
        }

        protected abstract int RestoresStamina { get; }

        protected abstract bool Perform(GameCore<Player> game, out Point newPosition);
    }
}