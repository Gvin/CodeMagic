using System;
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

        public Equipment()
        {
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

        public void EquipItem(IEquipableItem item)
        {
            switch (item)
            {
                case WeaponItem weaponItem:
                    EquipWeapon(weaponItem);
                    break;
                case ArmorItem armorItem:
                    EquipArmor(armorItem);
                    break;
                case SpellBook spellBookItem:
                    EquipSpellBook(spellBookItem);
                    break;
                default:
                    throw new ArgumentException($"Equipable item type is not supported: {item.GetType().FullName}");
            }
        }

        public void UnequipItem(IEquipableItem item)
        {
            switch (item)
            {
                case WeaponItem _:
                    UnequipWeapon();
                    break;
                case ArmorItem armorItem:
                    UnequipArmor(armorItem.ArmorType);
                    break;
                case SpellBook _:
                    UnequipSpellBook();
                    break;
                default:
                    throw new ArgumentException($"Equipable item type is not supported: {item.GetType().FullName}");
            }
        }

        private void UnequipArmor(ArmorType type)
        {
            var armor = Armor[type];
            if (armor != null)
            {
                Armor[type] = null;
            }
        }

        private void UnequipWeapon()
        {
            weapon = null;
        }

        private void UnequipSpellBook()
        {
            if (SpellBook != null)
            {
                SpellBook = null;
            }
        }

        private void EquipWeapon(WeaponItem newWeapon)
        {
            UnequipWeapon();
            weapon = newWeapon;
        }

        private void EquipArmor(ArmorItem newArmor)
        {
            UnequipArmor(newArmor.ArmorType);
            Armor[newArmor.ArmorType] = newArmor;
        }

        private void EquipSpellBook(SpellBook newSpellBook)
        {
            UnequipSpellBook();
            SpellBook = newSpellBook;
        }

        public bool IsEquiped(IEquipableItem item)
        {
            switch (item)
            {
                case WeaponItem weaponItem:
                    return weapon != null && weapon.Equals(weaponItem);
                case ArmorItem armorItem:
                    return Armor[armorItem.ArmorType] != null && Armor[armorItem.ArmorType].Equals(item);
                case SpellBook spellBookItem:
                    return SpellBook != null && SpellBook.Equals(spellBookItem);
                default:
                    throw new ArgumentException($"Equipable item type is not supported: {item.GetType().FullName}");
            }
        }

        public SpellBook SpellBook { get; private set; }

        public int Protection
        {
            get
            {
                return Armor.Select(pair => pair.Value).Where(armor => armor != null).Sum(armor => armor.Protection);
            }
        }
    }
}