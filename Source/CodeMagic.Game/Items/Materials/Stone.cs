using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Materials
{
    public class Stone : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string ResourceKey = "resource_stone";

        public Stone(SaveData data)
            : base(data)
        {
        }

        public Stone()
            : base(new ItemConfiguration
            {
                Key = ResourceKey,
                Name = "Stone",
                Rareness = ItemRareness.Trash,
                Weight = 3000
            })
        {
        }

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
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {{"A normal medium size stone.", TextHelper.DescriptionTextColor}}
            };
        }
    }
}