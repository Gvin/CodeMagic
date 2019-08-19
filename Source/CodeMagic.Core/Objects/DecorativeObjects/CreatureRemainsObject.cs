using CodeMagic.Core.Injection;

namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public interface ICreatureRemainsObject : IMapObject, IInjectable
    {
    }

    public abstract class CreatureRemainsObject : ICreatureRemainsObject
    {
        protected readonly RemainsType Type;

        public CreatureRemainsObject(CreatureRemainsObjectConfiguration configuration)
        {
            Type = configuration.Type;
        }

        #region IMapObject Implementation

        public abstract string Name { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

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

    public class CreatureRemainsObjectConfiguration
    {
        public CreatureRemainsObjectConfiguration(RemainsType type)
        {
            Type = type;
        }

        public RemainsType Type { get; set; }
    }

    public enum RemainsType
    {
        BloodRedSmall,
        BloodRedMedium,
        BloodRedBig,

        BloodGreenSmall,
        BloodGreenMedium,
        BloodGreenBig,

        BonesMedium
    }
}