using System;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.DecorativeObjects
{
    public class CreatureRemains : IMapObject, IWorldImageProvider
    {
        private const string ImageBloodSmall = "Remains_Blood_Small";
        private const string ImageBloodMedium = "Remains_Blood_Medium";
        private const string ImageBloodBig = "Remains_Blood_Big";

        private const string ImageGreenBloodSmall = "Remains_GreenBlood_Small";
        private const string ImageGreenBloodMedium = "Remains_GreenBlood_Medium";
        private const string ImageGreenBloodBig = "Remains_GreenBlood_Big";

        private const string ImageBonesWhiteSmall = "Remains_Bones_Small";
        private const string ImageBonesWhiteMedium = "Remains_Bones_Medium";

        protected readonly RemainsType Type;

        public CreatureRemains(RemainsType type)
        {
            Type = type;
            Name = GetName(type);
        }

        private static string GetName(RemainsType type)
        {
            switch (type)
            {
                case RemainsType.BloodRedSmall:
                case RemainsType.BloodRedMedium:
                case RemainsType.BloodRedBig:
                    return "Blood";
                case RemainsType.BloodGreenSmall:
                case RemainsType.BloodGreenMedium:
                case RemainsType.BloodGreenBig:
                    return "Green Blood";
                case RemainsType.BonesWhiteSmall:
                case RemainsType.BonesWhiteMedium:
                    return "Bones";
                default:
                    throw new ArgumentException($"Unknown remains type: {type}");
            }
        }

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

                case RemainsType.BonesWhiteSmall:
                    return ImageBonesWhiteSmall;
                case RemainsType.BonesWhiteMedium:
                    return ImageBonesWhiteMedium;

                default:
                    throw new ArgumentException($"Unknown remains type: {Type}");
            }
        }

        #region IMapObject Implementation

        public string Name { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool BlocksAttack => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.GroundDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        #endregion
    }

    public enum RemainsType
    {
        BloodRedSmall,
        BloodRedMedium,
        BloodRedBig,

        BloodGreenSmall,
        BloodGreenMedium,
        BloodGreenBig,

        BonesWhiteSmall,
        BonesWhiteMedium
    }
}