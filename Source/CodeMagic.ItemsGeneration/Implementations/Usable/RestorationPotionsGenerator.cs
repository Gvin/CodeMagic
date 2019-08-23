using System;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.Implementations.Items.Usable;

namespace CodeMagic.ItemsGeneration.Implementations.Usable
{
    public class RestorationPotionsGenerator : IUsableItemTypeGenerator
    {
        private readonly IImagesStorage imagesStorage;

        public RestorationPotionsGenerator(IImagesStorage imagesStorage)
        {
            this.imagesStorage = imagesStorage;
        }

        public IItem Generate(ItemRareness rareness)
        {
            switch (rareness)
            {
                case ItemRareness.Trash:
                    return null;
                case ItemRareness.Common:
                    return CreateSmallRestorationPotion();
                case ItemRareness.Uncommon:
                    return CreateMediumRestorationPotion();
                case ItemRareness.Rare:
                    return CreateBigRestorationPotion();
                case ItemRareness.Epic:
                    throw new ArgumentException("Potions generator cannot generate potions with Epic rareness.");
                default:
                    throw new ArgumentException($"Unknown rareness: {rareness}");
            }
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
    }
}