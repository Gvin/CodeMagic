using System;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.ObjectEffects
{
    public abstract class ObjectEffect : IObjectEffect
    {
        protected ObjectEffect()
        {
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; }

        public abstract SymbolsImage GetEffectImage(int width, int height, IImagesStorage imagesStorage);
    }
}