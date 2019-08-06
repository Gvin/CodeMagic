using System;

namespace CodeMagic.Core.Items
{
    public class Item : IItem
    {
        public Item(ItemConfiguration configuration)
        {
            Key = configuration.Key;
            Name = configuration.Name;
            Rareness = configuration.Rareness;
            Weight = configuration.Weight;

            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }

        public string Key { get; }

        public string Name { get; }
        public ItemRareness Rareness { get; }
        public int Weight { get; }
        public virtual bool Stackable => true;

        public bool Equals(IItem other)
        {
            if (Stackable)
            {
                return string.Equals(Key, other.Key);
            }

            return string.Equals(Id, other.Id);
        }
    }

    public class ItemConfiguration
    {
        public ItemConfiguration()
        {
            Rareness = ItemRareness.Trash;
            Weight = 1;
        }

        public string Key { get; set; }

        public string Name { get; set; }

        public ItemRareness Rareness { get; set; }

        public int Weight { get; set; }
    }
}