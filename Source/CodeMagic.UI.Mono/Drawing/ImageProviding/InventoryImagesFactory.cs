using CodeMagic.Core.Items;
using CodeMagic.Game;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Mono.Drawing.ImageProviding;

public interface IInventoryImagesFactory
{
    SymbolsImage GetImage(IItem item);
}

public class InventoryImagesFactory : IInventoryImagesFactory
{
    private readonly IImagesStorage _imagesStorage;

    public InventoryImagesFactory(IImagesStorage imagesStorage)
    {
        _imagesStorage = imagesStorage;
    }

    public SymbolsImage GetImage(IItem item)
    {
        if (item is not IInventoryImageProvider imageProvider)
        {
            return null;
        }

        return imageProvider.GetInventoryImage(_imagesStorage);
    }
}
