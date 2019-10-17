namespace CodeMagic.Game.JournalMessages
{
    public class RoofCollapsedMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {"A roof has collapsed"};
        }
    }
}