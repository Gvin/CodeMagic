namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class SpellOutOfManaMessage : IJournalMessage
    {
        public SpellOutOfManaMessage(string spellName)
        {
            SpellName = spellName;
        }

        public string SpellName { get; }
    }
}