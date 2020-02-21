using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable.Food
{
    public class Apple : FoodItemBase
    {
        public Apple() : base(3)
        {
        }

        public override string Name => "Apple";

        public override string Key => "food_apple";

        public override ItemRareness Rareness => ItemRareness.Common;

        public override int Weight => 100;

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