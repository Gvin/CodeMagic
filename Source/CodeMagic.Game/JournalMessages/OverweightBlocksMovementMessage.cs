namespace CodeMagic.Game.JournalMessages
{
    public class OverweightBlocksMovementMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine {$"Inventory is too heavy and {PlayerName} cannot move"};
        }
    }
}