using System;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Usable;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Usable
{
    public class HealthPotionsGenerator : IUsableItemTypeGenerator
    {
        private readonly IImagesStorage imagesStorage;

        public HealthPotionsGenerator(IImagesStorage imagesStorage)
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
                    return CreateSmallHealthPotion();
                case ItemRareness.Uncommon:
                    return CreateMediumHealthPotion();
                case ItemRareness.Rare:
                    return CreateBigHealthPotion();
                case ItemRareness.Epic:
                    throw new ArgumentException("Potions generator cannot generate potions with Epic rareness.");
                default:
                    throw new ArgumentException($"Unknown rareness: {rareness}");
            }
        }

        private Item CreateBigHealthPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = new[] {"Big jar with bloody-red liquid."},
                HealValue = 100,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Red_Big"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Red"),
                Key = "health_potion_big",
                Name = "Big Health Potion",
                Rareness = ItemRareness.Rare,
                Weight = 500
            });
        }

        private Item CreateMediumHealthPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = new[] {"Medium size jar with bloody-red liquid."},
                HealValue = 50,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Red"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Red"),
                Key = "health_potion",
                Name = "Health Potion",
                Rareness = ItemRareness.Uncommon,
                Weight = 300
            });
        }

        private Item CreateSmallHealthPotion()
        {
            return new HealthManaRestorationItem(new HealthManaRestorationItemConfiguration
            {
                Description = new[] {"A small phial with bloody-red liquid."},
                HealValue = 25,
                InventoryImage = imagesStorage.GetImage("Item_Potion_Red_Small"),
                WorldImage = imagesStorage.GetImage("ItemsOnGround_Potion_Red"),
                Key = "health_potion_small",
                Name = "Small Health Potion",
                Rareness = ItemRareness.Common,
                Weight = 150
            });
        }
    }
}