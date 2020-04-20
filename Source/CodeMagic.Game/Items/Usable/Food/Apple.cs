using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable.Food
{
    public class Apple : FoodItemBase
    {
        public Apple(SaveData data)
            : base(data)
        {
        }

        public Apple() 
            : base(3, new ItemConfiguration
            {
                Key = "food_apple",
                Name = "Apple",
                Rareness = ItemRareness.Common,
                Weight = 100
            })
        {
        }

        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Food_Apple");
        }

        public override SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Food_Apple");
        }

        protected override StyledLine[] GetDescriptionText()
        {
            return new[]
            {
                new StyledLine {{"A sweet red apple. Juicy and tasty.", TextHelper.DescriptionTextColor}}
            };
        }
    }
}