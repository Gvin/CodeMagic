using CodeMagic.UI.Images;

namespace CodeMagic.Game
{
    public interface IInventoryImageProvider
    {
        SymbolsImage GetInventoryImage(IImagesStorage storage);
    }
}