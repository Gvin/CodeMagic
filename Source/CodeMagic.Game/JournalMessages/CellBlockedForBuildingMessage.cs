namespace CodeMagic.Game.JournalMessages
{
    public class CellBlockedForBuildingMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {"Target area cannot be used for building"};
        }
    }
}