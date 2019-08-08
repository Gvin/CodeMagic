namespace CodeMagic.Core.Game.PlayerActions
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