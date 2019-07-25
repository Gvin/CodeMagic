using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation
{
    public interface IImageProvider
    {
        SymbolsImage GetImage(IImagesStorage storage);
    }
}