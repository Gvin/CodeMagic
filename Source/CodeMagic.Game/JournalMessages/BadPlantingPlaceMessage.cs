namespace CodeMagic.Game.JournalMessages
{
    public class BadPlantingPlaceMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine{ "You cannot plant here" };
        }
    }
}