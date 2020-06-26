namespace CodeMagic.Game.JournalMessages
{
    public class ContainerOpenMessage : SelfDescribingJournalMessage
    {
        private readonly string name;

        public ContainerOpenMessage(string name)
        {
            this.name = name;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine{$"{PlayerName} opened {name}"};
        }
    }
}