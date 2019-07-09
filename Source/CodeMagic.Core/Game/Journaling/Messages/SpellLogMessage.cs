namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class SpellLogMessage : IJournalMessage
    {
        public SpellLogMessage(string spellName, string message)
        {
            SpellName = spellName;
            Message = message;
        }

        public string SpellName { get; }

        public string Message { get; }
    }
}