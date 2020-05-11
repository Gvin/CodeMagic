namespace CodeMagic.Game.JournalMessages.Scenario
{
    public class StartGameMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine{$"{PlayerName} wake up on cold stone floor in strange room."};
        }
    }
}