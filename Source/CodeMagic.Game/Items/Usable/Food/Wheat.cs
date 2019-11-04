using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable.Food
{
    public class Wheat : FoodItemBase
    {
        public Wheat() : base(1)
        {
        }

        public override string Name => "Wheat";
        public override string Key => "food_wheat";
        public override ItemRareness Rareness => ItemRareness.Common;
        public override int Weight => 300;
        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Wheat");
        }

        public override SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Food_Wheat");
        }

        protected override StyledLine[] GetDescriptionText()
        {
            return new[]
            {
                new StyledLine {
                {
                    "Can be eaten when raw and used to make bread.", ItemTextHelper.DescriptionTextColor
                }}
            };
        }
    }
}