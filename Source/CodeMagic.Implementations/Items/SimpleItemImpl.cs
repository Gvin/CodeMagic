using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items
{
    public abstract class SimpleItemImpl : Item, IInventoryImageProvider
    {
        private readonly SymbolsImage inventoryImage;

        protected SimpleItemImpl(SimpleItemConfiguration configuration) 
            : base(configuration)
        {
            inventoryImage = configuration.InventoryImage;
        }
        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return inventoryImage;
        }
    }

    public class SimpleItemConfiguration : ItemConfiguration
    {
        public SymbolsImage InventoryImage { get; set; }
    }
}