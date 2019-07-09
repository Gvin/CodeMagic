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
        }

        public string Key { get; }

        public string Name { get; }
        public ItemRareness Rareness { get; }
        public int Weight { get; }
        public bool Stackable => true;
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