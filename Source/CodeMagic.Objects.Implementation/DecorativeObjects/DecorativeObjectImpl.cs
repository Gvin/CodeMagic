using System;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation.DecorativeObjects
{
    public class DecorativeObjectImpl : DecorativeObject, IImageProvider
    {
        private const string ImageBloodSmall = "Decoratives_Blood_Small";
        private const string ImageBloodMedium = "Decoratives_Blood_Medium";
        private const string ImageBloodBig = "Decoratives_Blood_Big";

        private const string ImageGreenBloodSmall = "Decoratives_GreenBlood_Small";
        private const string ImageGreenBloodMedium = "Decoratives_GreenBlood_Medium";
        private const string ImageGreenBloodBig = "Decoratives_GreenBlood_Big";

        public DecorativeObjectImpl(DecorativeObjectConfiguration configuration) 
            : base(configuration)
        {
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            switch (Type)
            {
                case DecorativeObjectConfiguration.ObjectTypeBloodSmall:
                    return storage.GetImage(ImageBloodSmall);
                case DecorativeObjectConfiguration.ObjectTypeBloodMedium:
                    return storage.GetImage(ImageBloodMedium);
                case DecorativeObjectConfiguration.ObjectTypeBloodBig:
                    return storage.GetImage(ImageBloodBig);

                case DecorativeObjectConfiguration.ObjectTypeGreenBloodSmall:
                    return storage.GetImage(ImageGreenBloodSmall);
                case DecorativeObjectConfiguration.ObjectTypeGreenBloodMedium:
                    return storage.GetImage(ImageGreenBloodMedium);
                case DecorativeObjectConfiguration.ObjectTypeGreenBloodBig:
                    return storage.GetImage(ImageGreenBloodBig);
                default:
                    throw new ApplicationException($"Unknown decorative object type: {Type}");
            }
        }
    }
}