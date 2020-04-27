using System.Collections.Generic;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public abstract class SimpleItem : Item, IInventoryImageProvider
    {
        private const string SaveKeyInventoryImage = "InventoryImage";

        private readonly SymbolsImage inventoryImage;

        protected SimpleItem(SaveData data) : base(data)
        {
            inventoryImage = data.GetObject<SymbolsImageSaveable>(SaveKeyInventoryImage)?.GetImage();
        }

        protected SimpleItem(SimpleItemConfiguration configuration) 
            : base(configuration)
        {
            inventoryImage = configuration.InventoryImage;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data =  base.GetSaveDataContent();
            data.Add(SaveKeyInventoryImage, inventoryImage != null ? new SymbolsImageSaveable(inventoryImage) : null);
            return data;
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