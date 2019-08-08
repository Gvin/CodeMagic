using CodeMagic.UI.Images;

namespace CodeMagic.Implementations
{
    public interface IImageProvider
    {
        SymbolsImage GetImage(IImagesStorage storage);
    }
}