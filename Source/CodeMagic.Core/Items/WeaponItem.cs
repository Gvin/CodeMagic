using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Items
{
    public class WeaponItem : Item, IEquipableItem
    {
        protected readonly Dictionary<Element, int> MaxDamage;
        protected readonly Dictionary<Element, int> MinDamage;

        public WeaponItem(WeaponItemConfiguration configuration) 
            : base(configuration)
        {
            MinDamage = configuration.MinDamage.ToDictionary(pair => pair.Key, pair => pair.Value);
            MaxDamage = configuration.MaxDamage.ToDictionary(pair => pair.Key, pair => pair.Value);

            HitChance = configuration.HitChance;
        }

        public Dictionary<Element, int> GenerateDamage()
        {
            return MaxDamage.ToDictionary(pair => pair.Key,
                pair => RandomHelper.GetRandomValue(pair.Value, MinDamage[pair.Key]));
        }

        public int HitChance { get; }

        public override bool Stackable => false;
    }

    public class WeaponItemConfiguration : ItemConfiguration
    {
        public Dictionary<Element, int> MaxDamage { get; set; }

        public Dictionary<Element, int> MinDamage { get; set; }

        public int HitChance { get; set; }
    }
}