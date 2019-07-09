namespace CodeMagic.Core.Objects.SolidObjects
{
    public class SolidObject : IMapObject
    {
        public SolidObject(SolidObjectConfiguration configuration)
        {
            Name = configuration.Name;
            Type = configuration.Type;
        }

        public string Name { get; }

        public string Type { get; }

        public bool BlocksMovement => true;

        public bool IsVisible => true;

        public bool BlocksVisibility => true;

        public bool BlocksProjectiles => true;
    }

    public class SolidObjectConfiguration
    {
        public const string ObjectTypeWallWood = "WallWood";
        public const string ObjectTypeWallStone = "WallStone";
        public const string ObjectTypeHole = "Hole";

        public string Name { get; set; }

        public string Type { get; set; }
    }
}