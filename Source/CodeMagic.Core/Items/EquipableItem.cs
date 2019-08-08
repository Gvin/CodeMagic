namespace CodeMagic.Core.Items
{
    public class EquipableItem : Item, IEquipableItem
    {
        public EquipableItem(EquipableItemConfiguration configuration) 
            : base(configuration)
        {
            HealthBonus = configuration.HealthBonus;
            ManaBonus = configuration.ManaBonus;
            ManaRegenerationBonus = configuration.ManaRegenerationBonus;
        }

        public int HealthBonus { get; }

        public int ManaBonus { get; }

        public int ManaRegenerationBonus { get; }
    }

    public class EquipableItemConfiguration : ItemConfiguration
    {
        public int HealthBonus { get; set; }

        public int ManaBonus { get; set; }

        public int ManaRegenerationBonus { get; set; }
    }
}