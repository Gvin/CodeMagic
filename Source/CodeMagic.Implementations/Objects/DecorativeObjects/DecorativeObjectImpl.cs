using System;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.DecorativeObjects
{
    public class DecorativeObjectImpl : DecorativeObject, IImageProvider
    {
        private const string ImageBloodSmall = "Decoratives_Blood_Small";
        private const string ImageBloodMedium = "Decoratives_Blood_Medium";
        private const string ImageBloodBig = "Decoratives_Blood_Big";

        private const string ImageGreenBloodSmall = "Decoratives_GreenBlood_Small";
        private const string ImageGreenBloodMedium = "Decoratives_GreenBlood_Medium";
        private const string ImageGreenBloodBig = "Decoratives_GreenBlood_Big";

        private const string ImageStonesSmall = "Decoratives_Stones_Small";

        public DecorativeObjectImpl(DecorativeObjectConfiguration configuration) 
            : base(configuration)
        {
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            switch (Type)
            {
                case DecorativeObjectConfiguration.ObjectType.BloodSmall:
                    return storage.GetImage(ImageBloodSmall);
                case DecorativeObjectConfiguration.ObjectType.BloodMedium:
                    return storage.GetImage(ImageBloodMedium);
                case DecorativeObjectConfiguration.ObjectType.BloodBig:
                    return storage.GetImage(ImageBloodBig);

                case DecorativeObjectConfiguration.ObjectType.GreenBloodSmall:
                    return storage.GetImage(ImageGreenBloodSmall);
                case DecorativeObjectConfiguration.ObjectType.GreenBloodMedium:
                    return storage.GetImage(ImageGreenBloodMedium);
                case DecorativeObjectConfiguration.ObjectType.GreenBloodBig:
                    return storage.GetImage(ImageGreenBloodBig);

                case DecorativeObjectConfiguration.ObjectType.StonesSmall:
                    return storage.GetImage(ImageStonesSmall);
                default:
                    throw new ApplicationException($"Unknown decorative object type: {Type}");
            }
        }
    }
}