namespace CodeMagic.Core.Items
{
    public class WeaponItem : Item, IEquipableItem
    {
        public WeaponItem(WeaponItemConfiguration configuration) 
            : base(configuration)
        {
            MinDamage = configuration.MinDamage;
            MaxDamage = configuration.MaxDamage;
            HitChance = configuration.HitChance;
        }

        public int MinDamage { get; }

        public int MaxDamage { get; }

        public int HitChance { get; }

        public override bool Stackable => false;
    }

    public class WeaponItemConfiguration : ItemConfiguration
    {
        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int HitChance { get; set; }
    }
}