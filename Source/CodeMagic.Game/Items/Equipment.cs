using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public class Equipment : ISaveable
    {
        private const string SaveKeyWeapon = "Weapon";
        private const string SaveKeyArmorHelmet = "ArmorHelmet";
        private const string SaveKeyArmorChest = "ArmorChest";
        private const string SaveKeyArmorLeggings = "ArmorLeggings";
        private const string SaveKeySpellBook = "SpellBook";

        private static readonly WeaponItem Fists = new WeaponItem(new WeaponItemConfiguration
        {
            MaxDamage = new Dictionary<Element, int> {{Element.Blunt, 2}},
            MinDamage = new Dictionary<Element, int> {{Element.Blunt, 0}},
            HitChance = 50,
            Name = "Fists",
            Key = "fists",
            Rareness = ItemRareness.Trash,
            Weight = 0
        });

        private WeaponItem weapon;

        public Equipment(SaveData data)
        {
            weapon = data.GetObject<WeaponItem>(SaveKeyWeapon);
            SpellBook = data.GetObject<SpellBook>(SaveKeySpellBook);
            Armor = new Dictionary<ArmorType, ArmorItem>
            {
                {ArmorType.Helmet, data.GetObject<ArmorItem>(SaveKeyArmorHelmet)},
                {ArmorType.Chest, data.GetObject<ArmorItem>(SaveKeyArmorChest)},
                {ArmorType.Leggings, data.GetObject<ArmorItem>(SaveKeyArmorLeggings)}
            };
        }

        public Equipment()
        {
            Armor = new Dictionary<ArmorType, ArmorItem>
            {
                {ArmorType.Helmet, null},
                {ArmorType.Chest, null},
                {ArmorType.Leggings, null}
            };
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyWeapon, weapon},
                {SaveKeySpellBook, SpellBook},
                {SaveKeyArmorHelmet, Armor[ArmorType.Helmet]},
                {SaveKeyArmorChest, Armor[ArmorType.Chest]},
                {SaveKeyArmorLeggings, Armor[ArmorType.Leggings]}
            });
        }

        public Dictionary<ArmorType, ArmorItem> Armor { get; }

        public WeaponItem Weapon => weapon ?? Fists;

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

        public int GetProtection(Element element)
        {
            return Armor.Sum(pair => pair.Value?.GetProtection(element) ?? 0);
        }

        public int GetBonus(EquipableBonusType bonusType)
        {
            var result = weapon?.GetBonus(bonusType) ?? 0;
            result += Armor.Where(pair => pair.Value != null).Sum(pair => pair.Value.GetBonus(bonusType));
            result += SpellBook?.GetBonus(bonusType) ?? 0;

            return result;
        }

        public int GetStatsBonus(PlayerStats statType)
        {
            var result = weapon?.GetStatBonus(statType) ?? 0;
            result += Armor.Where(pair => pair.Value != null).Sum(pair => pair.Value.GetStatBonus(statType));
            result += SpellBook?.GetStatBonus(statType) ?? 0;

            return result;
        }
    }
}