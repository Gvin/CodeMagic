namespace CodeMagic.Game.JournalMessages
{
    public class NotEnoughManaToScrollMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} have not enough ",
                ManaString,
                " to create a scroll with this spell"
            };
        }
    }
}