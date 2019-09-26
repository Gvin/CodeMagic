using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Materials
{
    public abstract class IngotBase : ItemBase, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string TemplateImageName = "Item_Resource_IngotTemplate";

        protected IngotBase(MetalType metalType)
        {
            MetalType = metalType;
        }

        public MetalType MetalType { get; }

        public sealed override bool Stackable => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Other");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            var template = storage.GetImage(TemplateImageName);
            return MetalRecolorHelper.RecolorMetalImage(template, MetalType);
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new []
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine{new StyledString("Standard metal ingot.", ItemTextHelper.DescriptionTextColor)} 
            };
        }
    }
}