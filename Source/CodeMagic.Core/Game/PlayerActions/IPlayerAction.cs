namespace CodeMagic.Core.Game.PlayerActions
{
    public interface IPlayerAction
    {
        bool Perform(out Point newPosition);
    }
}