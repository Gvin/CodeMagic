using System;

namespace CodeMagic.Core.Spells
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