using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items.Bonuses.Weapon;

namespace CodeMagic.Core.Items
{
    public class WeaponItem : Item, IEquipableItem
    {
        private readonly Dictionary<Element, int> maxDamage;
        private readonly Dictionary<Element, int> minDamage;

        public WeaponItem(WeaponItemConfiguration configuration) 
            : base(configuration)
        {
            minDamage = configuration.MinDamage.ToDictionary(pair => pair.Key, pair => pair.Value);
            maxDamage = configuration.MaxDamage.ToDictionary(pair => pair.Key, pair => pair.Value);

            HitChance = configuration.HitChance;
        }

        protected Dictionary<Element, int> MaxDamageValue
        {
            get
            {
                var result = new Dictionary<Element, int>(maxDamage);
                foreach (var weaponItemBonus in Bonuses.OfType<IWeaponItemBonus>())
                {
                    if (result.ContainsKey(weaponItemBonus.Element))
                    {
                        result[weaponItemBonus.Element] += weaponItemBonus.MaxDamage;
                    }
                    else
                    {
                        result.Add(weaponItemBonus.Element, weaponItemBonus.MaxDamage);
                    }
                }

                return result;
            }
        }

        protected Dictionary<Element, int> MinDamageValue
        {
            get
            {
                var result = new Dictionary<Element, int>(minDamage);
                foreach (var weaponItemBonus in Bonuses.OfType<IWeaponItemBonus>())
                {
                    if (result.ContainsKey(weaponItemBonus.Element))
                    {
                        result[weaponItemBonus.Element] += weaponItemBonus.MinDamage;
                    }
                    else
                    {
                        result.Add(weaponItemBonus.Element, weaponItemBonus.MinDamage);
                    }
                }

                return result;
            }
        }

        public Dictionary<Element, int> GenerateDamage()
        {
            return MaxDamageValue.ToDictionary(pair => pair.Key,
                pair => RandomHelper.GetRandomValue(pair.Value, MinDamageValue[pair.Key]));
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