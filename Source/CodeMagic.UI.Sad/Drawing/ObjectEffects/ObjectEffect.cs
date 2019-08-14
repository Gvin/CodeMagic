using System;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Sad.Drawing.ObjectEffects
{
    public abstract class ObjectEffect
    {
        protected ObjectEffect()
        {
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; }

        public abstract SymbolsImage GetEffectImage(int width, int height);
    }
}