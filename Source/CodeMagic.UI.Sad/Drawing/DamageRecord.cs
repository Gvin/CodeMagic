using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.UI.Sad.Drawing
{
    public class DamageRecord : IDamageRecord
    {
        public DamageRecord(int value, Element? element)
        {
            Value = value;
            Element = element;
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; }

        public int Value { get; }

        public Element? Element { get; }
    }
}