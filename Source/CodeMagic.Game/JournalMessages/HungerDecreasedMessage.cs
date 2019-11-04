namespace CodeMagic.Game.JournalMessages
{
    public class HungerDecreasedMessage : SelfDescribingJournalMessage
    {
        private readonly int decreaseValue;

        public HungerDecreasedMessage(int decreaseValue)
        {
            this.decreaseValue = decreaseValue;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine {$"{PlayerName} restored {decreaseValue}% of hunger"};
        }
    }
}