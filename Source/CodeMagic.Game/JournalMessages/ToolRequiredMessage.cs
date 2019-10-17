namespace CodeMagic.Game.JournalMessages
{
    public class ToolRequiredMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {"Specific tool is required for this action"};
        }
    }
}