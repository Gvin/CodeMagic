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
            BlocksMovement = configuration.BlocksMovement;
            ZIndex = configuration.ZIndex;
            Size = configuration.Size;
        }

        public ObjectSize Size { get; }

        public string Name { get; }

        public DecorativeObjectConfiguration.ObjectType Type { get; }

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
        public DecorativeObjectConfiguration()
        {
            ZIndex = ZIndex.GroundDecoration;
            Size = ObjectSize.Huge;
            BlocksMovement = false;
        }

        public string Name { get; set; }

        public ObjectType Type { get; set; }

        public bool BlocksMovement { get; set; }

        public ZIndex ZIndex { get; set; }

        public ObjectSize Size { get; set; }

        public enum ObjectType
        {
            StonesSmall,
            TrapDoor
        }
    }
}