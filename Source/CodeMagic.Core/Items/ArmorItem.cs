namespace CodeMagic.Core.Items
{
    public class ArmorItem : Item
    {
        public ArmorItem(ArmorItemConfiguration configuration) 
            : base(configuration)
        {
        }

        public ArmorType ArmorType { get; }

        public int Protection { get; }
    }

    public enum ArmorType
    {
        Head,
        Chest,
        Legs,
        Arms
    }

    public class ArmorItemConfiguration : ItemConfiguration
    {

    }
}