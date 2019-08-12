using System;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.DecorativeObjects
{
    public class CreatureRemainsObjectImpl : CreatureRemainsObject, IWorldImageProvider
    {
        private const string ImageBloodSmall = "Remains_Blood_Small";
        private const string ImageBloodMedium = "Remains_Blood_Medium";
        private const string ImageBloodBig = "Remains_Blood_Big";

        private const string ImageGreenBloodSmall = "Remains_GreenBlood_Small";
        private const string ImageGreenBloodMedium = "Remains_GreenBlood_Medium";
        private const string ImageGreenBloodBig = "Remains_GreenBlood_Big";

        public CreatureRemainsObjectImpl(CreatureRemainsObjectConfiguration configuration) : base(configuration)
        {
            Name = GetName(configuration.Type);
        }

        private static string GetName(RemainsType type)
        {
            switch (type)
            {
                case RemainsType.BloodRedSmall:
                case RemainsType.BloodRedMedium:
                case RemainsType.BloodRedBig:
                case RemainsType.BloodGreenSmall:
                case RemainsType.BloodGreenMedium:
                case RemainsType.BloodGreenBig:
                    return "Blood";
                default:
                    throw new ArgumentException($"Unknown remains type: {type}");
            }
        }

        public override string Name { get; }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var imageName = GetWorldImageName();
            return storage.GetImage(imageName);
        }

        private string GetWorldImageName()
        {
            switch (Type)
            {
                case RemainsType.BloodRedSmall:
                    return ImageBloodSmall;
                case RemainsType.BloodRedMedium:
                    return ImageBloodMedium;
                case RemainsType.BloodRedBig:
                    return ImageBloodBig;
                case RemainsType.BloodGreenSmall:
                    return ImageGreenBloodSmall;
                case RemainsType.BloodGreenMedium:
                    return ImageGreenBloodMedium;
                case RemainsType.BloodGreenBig:
                    return ImageGreenBloodBig;
                default:
                    throw new ArgumentException($"Unknown remains type: {Type}");
            }
        }
    }
}