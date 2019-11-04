using System;

namespace CodeMagic.Game.Spells
{
    public class SpellException : Exception
    {
        public SpellException(string message)
            : base(message)
        {
        }

        public SpellException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}