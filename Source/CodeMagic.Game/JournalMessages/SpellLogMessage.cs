namespace CodeMagic.Game.JournalMessages
{
    public class SpellLogMessage : SelfDescribingJournalMessage
    {
        private readonly string spellName;
        private readonly string message;

        public SpellLogMessage(string spellName, string message)
        {
            this.spellName = spellName;
            this.message = message;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                "Spell ",
                new StyledString(spellName, SpellNameColor),
                " sent a message: ",
                new StyledString(message, SpellLogMessageColor)
            };
        }
    }
}