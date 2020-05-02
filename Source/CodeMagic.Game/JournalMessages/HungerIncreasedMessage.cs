namespace CodeMagic.Game.JournalMessages
{
    public class HungerIncreasedMessage : SelfDescribingJournalMessage
    {
        private readonly int increase;

        public HungerIncreasedMessage(int increase)
        {
            this.increase = increase;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine{$"Your hunger increased by {increase}%"};
        }
    }
}