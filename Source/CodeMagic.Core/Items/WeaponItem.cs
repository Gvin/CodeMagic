namespace CodeMagic.Core.Items
{
    public class WeaponItem : Item
    {
        public WeaponItem(WeaponItemConfiguration configuration) 
            : base(configuration)
        {
            DamageMin = configuration.DamageMin;
            DamageMax = configuration.DamageMax;
        }

        public int DamageMin { get; }

        public int DamageMax { get; }
    }

    public class WeaponItemConfiguration : ItemConfiguration
    {
        public int DamageMin { get; set; }

        public int DamageMax { get; set; }
    }
}