using System;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.DecorativeObjects
{
    public class DecorativeObjectImpl : DecorativeObject, IWorldImageProvider
    {
        private const string ImageStonesSmall = "Decoratives_Stones_Small";

        public DecorativeObjectImpl(DecorativeObjectConfiguration configuration) 
            : base(configuration)
        {
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            switch (Type)
            {
                case DecorativeObjectConfiguration.ObjectType.StonesSmall:
                    return storage.GetImage(ImageStonesSmall);
                default:
                    throw new ApplicationException($"Unknown decorative object type: {Type}");
            }
        }
    }
}