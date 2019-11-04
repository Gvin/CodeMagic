namespace CodeMagic.Game.JournalMessages
{
    public class CastNotAllowedMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {"Casting is not allowed in this area"};
        }
    }
}