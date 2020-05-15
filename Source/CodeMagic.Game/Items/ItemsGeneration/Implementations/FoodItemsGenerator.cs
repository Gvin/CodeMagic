using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Usable.Food;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations
{
    public class FoodItemsGenerator
    {
        private static readonly Func<IImagesStorage, IItem>[] Generators = {
            CreateApple,
            CreateMeat
        };

        private readonly IImagesStorage imagesStorage;

        public FoodItemsGenerator(IImagesStorage imagesStorage)
        {
            this.imagesStorage = imagesStorage;
        }

        public IItem GenerateFood()
        {
            var generator = RandomHelper.GetRandomElement(Generators);
            return generator(imagesStorage);
        }

        private static IItem CreateMeat(IImagesStorage storage)
        {
            return new FoodItem(new FoodItemConfiguration
            {
                Key = "food_meat",
                Name = "Meat",
                HungerDecrease = 7,
                Rareness = ItemRareness.Common,
                Weight = 500,
                InventoryImage = storage.GetImage("Food_Meat"),
                WorldImage = storage.GetImage("ItemsOnGround_Food_Meat"),
                Description = new[]
                {
                    "A big piece of meat. It smells good."
                }
            });
        }

        private static IItem CreateApple(IImagesStorage storage)
        {
            return new FoodItem(new FoodItemConfiguration
            {
                Key = "food_apple",
                Name = "Apple",
                HungerDecrease = 3,
                Rareness = ItemRareness.Common,
                Weight = 200,
                InventoryImage = storage.GetImage("Food_Apple"),
                WorldImage = storage.GetImage("ItemsOnGround_Food_Apple"),
                Description = new []
                {
                    "A sweet red apple. Juicy and tasty."
                }
            });
        }
    }
}