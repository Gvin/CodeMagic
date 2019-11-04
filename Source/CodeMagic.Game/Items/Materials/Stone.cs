using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Materials
{
    public class Stone : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string ResourceKey = "resource_stone";

        public override string Name => "Stone";

        public override string Key => ResourceKey;

        public override ItemRareness Rareness => ItemRareness.Common;

        public override int Weight => 3000;

        public override bool Stackable => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("Decoratives_Stones_Small");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Item_Resource_Stone");
        }

        public StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {{"A normal medium size stone.", ItemTextHelper.DescriptionTextColor}},
                new StyledLine {{"It can be used for building.", ItemTextHelper.DescriptionTextColor}}
            };
        }
    }
}