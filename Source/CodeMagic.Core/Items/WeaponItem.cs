namespace CodeMagic.Core.Items
{
    public class WeaponItem : Item
    {
        private int health;

        public WeaponItem(WeaponItemConfiguration configuration) 
            : base(configuration)
        {
            DamageMin = configuration.DamageMin;
            DamageMax = configuration.DamageMax;
            MaxHealth = configuration.MaxHealth;
            Health = configuration.Health ?? MaxHealth;
        }

        public int MaxHealth { get; }

        public int Health
        {
            get => health;
            set
            {
                if (value < 0)
                {
                    health = 0;
                    return;
                }

                if (value > MaxHealth)
                {
                    health = MaxHealth;
                    return;
                }

                health = value;
            }
        }

        public int DamageMin { get; }

        public int DamageMax { get; }
    }

    public class WeaponItemConfiguration : ItemConfiguration
    {
        public int DamageMin { get; set; }

        public int DamageMax { get; set; }

        public int MaxHealth { get; set; }

        public int? Health { get; set; }
    }
}