namespace CodeMagic.Game.JournalMessages
{
    public class StoringLocationNotAllowedMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {"This area is too unstable to store its location."};
        }
    }
}