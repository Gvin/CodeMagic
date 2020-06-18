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
        private const string SaveKeyRightHandItem = "RightHandItem";
        private const string SaveKeyLeftHandItem = "LeftHandItem";
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

        private IHoldableItem rightHandItem;
        private IHoldableItem leftHandItem;

        public Equipment(SaveData data, Inventory inventory)
        {
            var rightWeaponId = data.GetStringValue(SaveKeyRightHandItem);
            if (rightWeaponId != null)
            {
                rightHandItem = (IHoldableItem) inventory.GetItemById(rightWeaponId);
            }

            var leftWeaponId = data.GetStringValue(SaveKeyLeftHandItem);
            if (leftWeaponId != null)
            {
                leftHandItem = (IHoldableItem) inventory.GetItemById(leftWeaponId);
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
                {SaveKeyRightHandItem, rightHandItem?.Id},
                {SaveKeyLeftHandItem, leftHandItem?.Id},
                {SaveKeySpellBook, SpellBook?.Id},
                {SaveKeyArmorHelmet, Armor[ArmorType.Helmet]?.Id},
                {SaveKeyArmorChest, Armor[ArmorType.Chest]?.Id},
                {SaveKeyArmorLeggings, Armor[ArmorType.Leggings]?.Id}
            });
        }

        public bool RightHandItemEquipped => rightHandItem != null;

        public bool LeftHandItemEquipped => leftHandItem != null;

        public int HitChanceBonus => GetEquippedItems().OfType<ShieldItem>().Sum(shield => shield.HitChancePenalty);

        public IEquipableItem[] GetEquippedItems()
        {
            var result = new List<IEquipableItem>
            {
                SpellBook,
                leftHandItem,
                rightHandItem
            };

            result.AddRange(Armor.Values);

            return result.Where(item => item != null).ToArray();
        }

        public Dictionary<ArmorType, IArmorItem> Armor { get; }

        public IHoldableItem RightHandItem => rightHandItem ?? Fists;

        public IHoldableItem LeftHandItem => leftHandItem ?? Fists;

        public bool IsDoubleWielded => rightHandItem == null || leftHandItem == null;

        public void EquipHoldable(IHoldableItem holdable, bool isRight)
        {
            if (isRight)
            {
                rightHandItem = holdable;
            }
            else
            {
                leftHandItem = holdable;
            }
        }

        public void EquipItem(IEquipableItem item)
        {
            switch (item)
            {
                case IHoldableItem _:
                    throw new ArgumentException($"Weapon items should be equiped with {nameof(EquipHoldable)}");
                case IArmorItem armorItem:
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
                case IHoldableItem holdableItem:
                    UnequipHoldable(holdableItem);
                    break;
                case IArmorItem armorItem:
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

        private void UnequipHoldable(IHoldableItem holdable)
        {
            if (rightHandItem != null && rightHandItem.Equals(holdable))
            {
                rightHandItem = null;
            }
            else if (leftHandItem != null && leftHandItem.Equals(holdable))
            {
                leftHandItem = null;
            }
            else
            {
                throw new ArgumentException($"Weapon {holdable.Key} is not equiped");
            }
        }

        private void UnequipSpellBook()
        {
            if (SpellBook != null)
            {
                SpellBook = null;
            }
        }

        private void EquipArmor(IArmorItem newArmor)
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
                case IHoldableItem holdableItem:
                    return (rightHandItem != null && rightHandItem.Equals(holdableItem)) || (leftHandItem != null && leftHandItem.Equals(holdableItem));
                case IArmorItem armorItem:
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