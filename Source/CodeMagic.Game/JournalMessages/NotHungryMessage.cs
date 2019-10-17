namespace CodeMagic.Game.JournalMessages
{
    public class NotHungryMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {$"{PlayerName} are not hungry enough to eat something"};
        }
    }
}