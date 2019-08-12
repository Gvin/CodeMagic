using CodeMagic.UI.Images;

namespace CodeMagic.Implementations
{
    public interface IInventoryImageProvider
    {
        SymbolsImage GetInventoryImage(IImagesStorage storage);
    }
}