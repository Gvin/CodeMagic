namespace CodeMagic.Game.JournalMessages
{
    public class FailedToUseScrollMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {$"{PlayerName} failed to use scroll. It's destroyed"};
        }
    }
}