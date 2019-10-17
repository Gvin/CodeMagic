using CodeMagic.Core.Items;
using CodeMagic.Game;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Sad.Drawing.ImageProviding
{
    public class InventoryImagesFactory
    {
        private readonly IImagesStorage imagesStorage;

        public InventoryImagesFactory(IImagesStorage imagesStorage)
        {
            this.imagesStorage = imagesStorage;
        }

        public SymbolsImage GetImage(IItem item)
        {
            if (!(item is IInventoryImageProvider imageProvider))
                return null;

            return imageProvider.GetInventoryImage(imagesStorage);
        }
    }
}