using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.Implementations.Items.Usable;

namespace CodeMagic.ItemsGeneration.Implementations
{
    public class PotionsGenerator
    {
        private readonly IImagesStorage imagesStorage;

        public PotionsGenerator(IImagesStorage imagesStorage)
        {
            this.imagesStorage = imagesStorage;
        }

        public Item GeneratePotion(ItemRareness rareness)
        {
            var type = GetRandomPotionType();
            switch (rareness)
            {
                case ItemRareness.Trash:
                    return null;
                case ItemRareness.Common:
                    return CreateSmallPotion(type);
                case ItemRareness.Uncommon:
                    return CreateMediumPotion(type);
                case ItemRareness.Rare:
                    return CreateBigPotion(type);
                case ItemRareness.Epic:
                    throw new ArgumentException("Potions generator cannot generate potions with Epic rareness.");
                default:
                    throw new ArgumentException($"Unknown rareness: {rareness}");
            }
        }

        private Item CreateSmallPotion(PotionType type)
        {
            switch (type)
            {
                case PotionType.Health:
                    return CreateSmallHealthPotion();
                case PotionType.Mana:
                    return CreateSmallManaPotion();
                default:
                    throw new ArgumentException($"Unknown potion type: {type}");
            }
        }

        private Item CreateMediumPotion(PotionType type)
        {
            switch (type)
            {
                case PotionType.Health:
                    return CreateMediumHealthPotion();
                case PotionType.Mana:
                    return CreateMediumManaPotion();
                default:
                    throw new ArgumentException($"Unknown potion type: {type}");
            }
        }

        private Item CreateBigPotion(PotionType type)
        {
            switch (type)
            {
                case PotionType.Health:
                    return CreateBigHealthPotion();
                case PotionType.Mana:
                    return CreateBigManaPotion();
                default:
                    throw new ArgumentException($"Unknown potion type: {type}");
            }
        }

        private Item CreateBigHealthPotion()
        {
            return new HealthRestorationItem(new HealthPotionItemConfiguration
            {
                Description = "Big jar with bloody-red liquid.",
                HealValue = 100,
                ImageName = "Item_Potion_Red",
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Red"),
                Key = "health_potion",
                Name = "Health Potion",
                Rareness = ItemRareness.Rare,
                Weight = 1
            });
        }

        private Item CreateBigManaPotion()
        {
            return new ManaRestorationItem(new ManaRestorationItemConfiguration
            {
                Description = "Big jar with bright blue liquid.",
                ManaRestoreValue = 100,
                ImageName = "Item_Potion_Blue",
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Blue"),
                Key = "mana_potion",
                Name = "Mana Potion",
                Rareness = ItemRareness.Uncommon,
                Weight = 1
            });
        }

        private Item CreateMediumHealthPotion()
        {
            return new HealthRestorationItem(new HealthPotionItemConfiguration
            {
                Description = "Medium sized jar with bloody-red liquid.",
                HealValue = 50,
                ImageName = "Item_Potion_Red",
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Red"),
                Key = "health_potion",
                Name = "Health Potion",
                Rareness = ItemRareness.Uncommon,
                Weight = 1
            });
        }

        private Item CreateMediumManaPotion()
        {
            return new ManaRestorationItem(new ManaRestorationItemConfiguration
            {
                Description = "Medium sized jar with bright blue liquid.",
                ManaRestoreValue = 50,
                ImageName = "Item_Potion_Blue",
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Blue"),
                Key = "mana_potion",
                Name = "Mana Potion",
                Rareness = ItemRareness.Uncommon,
                Weight = 1
            });
        }

        private Item CreateSmallHealthPotion()
        {
            return new HealthRestorationItem(new HealthPotionItemConfiguration
            {
                Description = "A small phial with bloody-red liquid.",
                HealValue = 25,
                ImageName = "Item_Potion_Red_Small",
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Red"),
                Key = "health_potion_small",
                Name = "Small Health Potion",
                Rareness = ItemRareness.Common,
                Weight = 1
            });
        }

        private Item CreateSmallManaPotion()
        {
            return new ManaRestorationItem(new ManaRestorationItemConfiguration
            {
                Description = "A small phial with bright blue liquid.",
                ManaRestoreValue = 25,
                ImageName = "Item_Potion_Blue_Small",
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Blue"),
                Key = "mana_potion_small",
                Name = "Small Mana Potion",
                Rareness = ItemRareness.Common,
                Weight = 1
            });
        }

        private PotionType GetRandomPotionType()
        {
            var types = Enum.GetValues(typeof(PotionType)).OfType<PotionType>().ToArray();
            return RandomHelper.GetRandomElement(types);
        }

        private enum PotionType
        {
            Health = 0,
            Mana = 1
        }
    }
}