using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Core.Items
{
    public class Equipment
    {
        private static readonly WeaponItem Fists = new WeaponItem(new WeaponItemConfiguration
        {
            MaxDamage = 2,
            MinDamage = 0,
            HitChance = 50,
            Name = "Fists",
            Rareness = ItemRareness.Trash,
            Weight = 0
        });

        private WeaponItem weapon;
        private readonly Inventory inventory;

        public Equipment(Inventory inventory)
        {
            this.inventory = inventory;

            Armor = new Dictionary<ArmorType, ArmorItem>
            {
                {ArmorType.Arms, null},
                {ArmorType.Head, null},
                {ArmorType.Chest, null},
                {ArmorType.Legs, null}
            };
        }

        public Dictionary<ArmorType, ArmorItem> Armor { get; }

        public WeaponItem Weapon
        {
            get => weapon ?? Fists;
        }

        public void EquipWeapon(WeaponItem newWeapon)
        {
            if (weapon != null)
            {
                inventory.AddItem(weapon);
            }

            weapon = newWeapon;
        }

        public SpellBook SpellBook { get; set; }

        public int Protection
        {
            get
            {
                return Armor.Select(pair => pair.Value).Where(armor => armor != null).Sum(armor => armor.Protection);
            }
        }
    }
}