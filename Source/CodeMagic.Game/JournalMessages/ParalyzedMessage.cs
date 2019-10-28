namespace CodeMagic.Game.JournalMessages
{
    public class ParalyzedMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {$"{PlayerName} paralyzed and cannot move or attack"};
        }
    }
}