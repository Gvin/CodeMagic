namespace CodeMagic.Game.JournalMessages
{
    public class BuildingSiteRemovedMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {"Building site removed"};
        }
    }
}