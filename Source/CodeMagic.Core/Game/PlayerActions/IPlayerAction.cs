namespace CodeMagic.Core.Game.PlayerActions
{
    public interface IPlayerAction
    {
        bool Perform(IGameCore game, out Point newPosition);
    }
}