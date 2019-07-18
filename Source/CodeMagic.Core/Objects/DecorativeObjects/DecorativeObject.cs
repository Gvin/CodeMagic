namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public class DecorativeObject : IMapObject
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
    }

    public class DecorativeObjectConfiguration
    {
        public const string ObjectTypeBloodSmall = "BloodSmall";
        public const string ObjectTypeBloodMedium = "BloodMedium";
        public const string ObjectTypeBloodBig = "BloodBig";

        public const string ObjectTypeGoblinBloodSmall = "GoblinBloodSmall";
        public const string ObjectTypeGoblinBloodMedium = "GoblinBloodMedium";
        public const string ObjectTypeGoblinBloodBig = "GoblinBloodBig";

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