namespace CodeMagic.Game.JournalMessages
{
    public class NotEnoughManaToCastMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} have not enough ",
                ManaString,
                " to cast this spell"
            };
        }
    }
}