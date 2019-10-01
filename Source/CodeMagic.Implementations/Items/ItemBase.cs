using System;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;

namespace CodeMagic.Implementations.Items
{
    public abstract class ItemBase : IItem
    {
        protected ItemBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        public abstract string Name { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public bool BlocksAttack => false;

        public ZIndex ZIndex => ZIndex.GroundDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Small;

        public string Id { get; }

        public abstract string Key { get; }

        public abstract ItemRareness Rareness { get; }

        public abstract int Weight { get; }

        public abstract bool Stackable { get; }

        public bool Equals(IItem other)
        {
            if (other == null)
                return false;

            if (Stackable)
            {
                return string.Equals(Key, other.Key);
            }

            return string.Equals(Id, other.Id);
        }
    }
}