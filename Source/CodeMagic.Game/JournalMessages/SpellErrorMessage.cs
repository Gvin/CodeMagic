namespace CodeMagic.Game.JournalMessages
{
    public class SpellErrorMessage : SelfDescribingJournalMessage
    {
        private readonly string spellName;
        private readonly string message;

        public SpellErrorMessage(string spellName, string message)
        {
            this.spellName = spellName;
            this.message = message;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                "An error occured in spell ",
                new StyledString(spellName, SpellNameColor),
                ": ",
                new StyledString(message, ErrorColor)
            };
        }
    }
}