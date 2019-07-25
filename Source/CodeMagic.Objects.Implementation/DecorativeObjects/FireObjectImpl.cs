using System;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation.DecorativeObjects
{
    public class FireObjectImpl : FireDecorativeObject, IImageProvider
    {
        private const string ImageSmall = "Fire_Small";
        private const string ImageMedium = "Fire_Medium";
        private const string ImageBig = "Fire_Big";

        public FireObjectImpl(int temperature) : base(temperature)
        {
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            switch (Type)
            {
                case ObjectTypeSmallFire:
                    return storage.GetImage(ImageSmall);
                case ObjectTypeMediumFire:
                    return storage.GetImage(ImageMedium);
                case ObjectTypeBigFire:
                    return storage.GetImage(ImageBig);
                default:
                    throw new ApplicationException($"Unknown fire type: {Type}");
            }
        }
    }
}