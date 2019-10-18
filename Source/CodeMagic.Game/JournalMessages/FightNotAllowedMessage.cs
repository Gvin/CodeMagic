namespace CodeMagic.Game.JournalMessages
{
    public class FightNotAllowedMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {"Fighting is not allowed in this area"};
        }
    }
}