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
                case PotionType.Restoration:
                    return CreateSmallRestorationPotion();
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
                case PotionType.Restoration:
                    return CreateMediumRestorationPotion();
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
                case PotionType.Restoration:
                    return CreateBigRestorationPotion();
                default:
                    throw new ArgumentException($"Unknown potion type: {type}");
            }
        }

        private Item CreateBigHealthPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = "Big jar with bloody-red liquid.",
                HealValue = 100,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Red_Big"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Red"),
                Key = "health_potion_big",
                Name = "Big Health Potion",
                Rareness = ItemRareness.Rare,
                Weight = 1
            });
        }

        private Item CreateBigManaPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = "Big jar with bright blue liquid.",
                ManaRestoreValue = 1000,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Blue_Big"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Blue"),
                Key = "mana_potion_big",
                Name = "Big Mana Potion",
                Rareness = ItemRareness.Rare,
                Weight = 1
            });
        }

        private Item CreateBigRestorationPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = "Big jar with bright purple liquid.",
                ManaRestoreValue = 600,
                HealValue = 60,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Purple_Big"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Purple"),
                Key = "restoration_potion_big",
                Name = "Big Restoration Potion",
                Rareness = ItemRareness.Rare,
                Weight = 1
            });
        }

        private Item CreateMediumHealthPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = "Medium size jar with bloody-red liquid.",
                HealValue = 50,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Red"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Red"),
                Key = "health_potion",
                Name = "Health Potion",
                Rareness = ItemRareness.Uncommon,
                Weight = 1
            });
        }

        private Item CreateMediumManaPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = "Medium size jar with bright blue liquid.",
                ManaRestoreValue = 500,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Blue"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Blue"),
                Key = "mana_potion",
                Name = "Mana Potion",
                Rareness = ItemRareness.Uncommon,
                Weight = 1
            });
        }

        private Item CreateMediumRestorationPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = "Medium size jar with bright purple liquid.",
                ManaRestoreValue = 400,
                HealValue = 40,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Purple"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Purple"),
                Key = "restoration_potion",
                Name = "Restoration Potion",
                Rareness = ItemRareness.Uncommon,
                Weight = 1
            });
        }

        private Item CreateSmallHealthPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = "A small phial with bloody-red liquid.",
                HealValue = 25,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Red_Small"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Red"),
                Key = "health_potion_small",
                Name = "Small Health Potion",
                Rareness = ItemRareness.Common,
                Weight = 1
            });
        }

        private Item CreateSmallManaPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = "A small phial with bright blue liquid.",
                ManaRestoreValue = 250,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Blue_Small"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Blue"),
                Key = "mana_potion_small",
                Name = "Small Mana Potion",
                Rareness = ItemRareness.Common,
                Weight = 1
            });
        }

        private Item CreateSmallRestorationPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = "A small phial with bright purple liquid.",
                ManaRestoreValue = 200,
                HealValue = 20,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Purple_Small"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Purple"),
                Key = "restoration_potion_small",
                Name = "Small Restoration Potion",
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
            Health,
            Mana,
            Restoration
        }
    }
}