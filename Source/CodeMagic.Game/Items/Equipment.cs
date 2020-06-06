using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public class Equipment : ISaveable
    {
        private const string SaveKeyRightWeapon = "RightWeapon";
        private const string SaveKeyLeftWeapon = "LeftWeapon";
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
            Key = "default_fists",
            Rareness = ItemRareness.Trash,
            Weight = 0
        });

        private IWeaponItem rightWeapon;
        private IWeaponItem leftWeapon;

        public Equipment(SaveData data, Inventory inventory)
        {
            var rightWeaponId = data.GetStringValue(SaveKeyRightWeapon);
            if (rightWeaponId != null)
            {
                rightWeapon = (IWeaponItem) inventory.GetItemById(rightWeaponId);
            }

            var leftWeaponId = data.GetStringValue(SaveKeyLeftWeapon);
            if (leftWeaponId != null)
            {
                leftWeapon = (IWeaponItem) inventory.GetItemById(leftWeaponId);
            }

            var spellBookId = data.GetStringValue(SaveKeySpellBook);
            if (spellBookId != null)
            {
                SpellBook = (SpellBook) inventory.GetItemById(spellBookId);
            }

            Armor = new Dictionary<ArmorType, IArmorItem>
            {
                {ArmorType.Helmet, null},
                {ArmorType.Chest, null},
                {ArmorType.Leggings, null}
            };

            var helmetId = data.GetStringValue(SaveKeyArmorHelmet);
            if (helmetId != null)
            {
                Armor[ArmorType.Helmet] = (IArmorItem) inventory.GetItemById(helmetId);
            }

            var chestId = data.GetStringValue(SaveKeyArmorChest);
            if (chestId != null)
            {
                Armor[ArmorType.Chest] = (IArmorItem)inventory.GetItemById(chestId);
            }

            var leggingsId = data.GetStringValue(SaveKeyArmorLeggings);
            if (leggingsId != null)
            {
                Armor[ArmorType.Leggings] = (IArmorItem)inventory.GetItemById(leggingsId);
            }
        }

        public Equipment()
        {
            Armor = new Dictionary<ArmorType, IArmorItem>
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
                {SaveKeyRightWeapon, rightWeapon?.Id},
                {SaveKeyLeftWeapon, leftWeapon?.Id},
                {SaveKeySpellBook, SpellBook?.Id},
                {SaveKeyArmorHelmet, Armor[ArmorType.Helmet]?.Id},
                {SaveKeyArmorChest, Armor[ArmorType.Chest]?.Id},
                {SaveKeyArmorLeggings, Armor[ArmorType.Leggings]?.Id}
            });
        }

        public IEquipableItem[] GetEquippedItems()
        {
            var result = new List<IEquipableItem>
            {
                SpellBook,
                leftWeapon,
                rightWeapon
            };

            result.AddRange(Armor.Values);

            return result.Where(item => item != null).ToArray();
        }

        public Dictionary<ArmorType, IArmorItem> Armor { get; }

        public IWeaponItem RightWeapon => rightWeapon ?? Fists;

        public IWeaponItem LeftWeapon => leftWeapon ?? Fists;

        public bool IsDoubleWielded => rightWeapon == null || leftWeapon == null;

        public void EquipWeapon(IWeaponItem weapon, bool isRight)
        {
            if (isRight)
            {
                rightWeapon = weapon;
            }
            else
            {
                leftWeapon = weapon;
            }
        }

        public void EquipItem(IEquipableItem item)
        {
            switch (item)
            {
                case WeaponItem _:
                    throw new ArgumentException($"Weapon items should be equiped with {nameof(EquipWeapon)}");
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
                case WeaponItem weaponItem:
                    UnequipWeapon(weaponItem);
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

        private void UnequipWeapon(WeaponItem weapon)
        {
            if (rightWeapon != null && rightWeapon.Equals(weapon))
            {
                rightWeapon = null;
            }
            else if (leftWeapon != null && leftWeapon.Equals(weapon))
            {
                leftWeapon = null;
            }
            else
            {
                throw new ArgumentException($"Weapon {weapon.Key} is not equiped");
            }
        }

        private void UnequipSpellBook()
        {
            if (SpellBook != null)
            {
                SpellBook = null;
            }
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
                    return (rightWeapon != null && rightWeapon.Equals(weaponItem)) || (leftWeapon != null && leftWeapon.Equals(weaponItem));
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
            return GetEquippedItems().Sum(item => item.GetBonus(bonusType));
        }

        public int GetStatsBonus(PlayerStats statType)
        {
            return GetEquippedItems().Sum(item => item.GetStatBonus(statType));
        }

        public ILightSource[] GetLightSources()
        {
            return GetEquippedItems().OfType<ILightSource>().ToArray();
        }
    }
}