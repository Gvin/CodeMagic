using System;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.Implementations.Items.Usable;

namespace CodeMagic.ItemsGeneration.Implementations.Usable
{
    public class ManaPotionsGenerator : IUsableItemTypeGenerator
    {
        private readonly IImagesStorage imagesStorage;

        public ManaPotionsGenerator(IImagesStorage imagesStorage)
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
                    return CreateSmallManaPotion();
                case ItemRareness.Uncommon:
                    return CreateMediumManaPotion();
                case ItemRareness.Rare:
                    return CreateBigManaPotion();
                case ItemRareness.Epic:
                    throw new ArgumentException("Potions generator cannot generate potions with Epic rareness.");
                default:
                    throw new ArgumentException($"Unknown rareness: {rareness}");
            }
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
    }
}