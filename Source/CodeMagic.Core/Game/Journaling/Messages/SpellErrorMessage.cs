namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class SpellErrorMessage : IJournalMessage
    {
        public SpellErrorMessage(string spellName, string message)
        {
            SpellName = spellName;
            Message = message;
        }

        public string SpellName { get; }

        public string Message { get; }
    }
}