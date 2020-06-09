namespace CodeMagic.Game.JournalMessages
{
    public class StaminaRestoredMessage : SelfDescribingJournalMessage
    {
        private readonly int restored;

        public StaminaRestoredMessage(int restored)
        {
            this.restored = restored;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} restored ",
                new StyledString(restored, StaminaColor),
                " stamina"
            };
        }
    }
}