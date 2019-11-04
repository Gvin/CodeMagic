namespace CodeMagic.Game.JournalMessages
{
    public class SpellOutOfManaMessage : SelfDescribingJournalMessage
    {
        private readonly string spellName;

        public SpellOutOfManaMessage(string spellName)
        {
            this.spellName = spellName;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                "Spell ",
                new StyledString(spellName, SpellNameColor),
                " is out of ",
                ManaString
            };
        }
    }
}