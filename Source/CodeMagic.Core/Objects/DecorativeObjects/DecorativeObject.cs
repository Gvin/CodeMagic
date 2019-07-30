using CodeMagic.Core.Injection;

namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public interface IDecorativeObject : IMapObject, IInjectable
    {
    }

    public class DecorativeObject : IDecorativeObject
    {
        public DecorativeObject(DecorativeObjectConfiguration configuration)
        {
            Name = configuration.Name;
            Type = configuration.Type;
            BlocksMovement = configuration.IsBigObject;
            ZIndex = configuration.ZIndex;
        }

        public string Name { get; }

        public string Type { get; }

        public bool BlocksMovement { get; }

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksProjectiles => false;

        public bool BlocksEnvironment => false;

        public virtual ZIndex ZIndex { get; }

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }
    }

    public class DecorativeObjectConfiguration
    {
        public const string ObjectTypeBloodSmall = "BloodSmall";
        public const string ObjectTypeBloodMedium = "BloodMedium";
        public const string ObjectTypeBloodBig = "BloodBig";

        public const string ObjectTypeGreenBloodSmall = "GreenBloodSmall";
        public const string ObjectTypeGreenBloodMedium = "GreenBloodMedium";
        public const string ObjectTypeGreenBloodBig = "GreenBloodBig";

        public const string ObjectTypeWoodPieces = "WoodPieces";

        public DecorativeObjectConfiguration()
        {
            ZIndex = ZIndex.GroundDecoration;
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsBigObject { get; set; }

        public ZIndex ZIndex { get; set; }
    }
}