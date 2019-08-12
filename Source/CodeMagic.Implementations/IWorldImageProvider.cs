using CodeMagic.UI.Images;

namespace CodeMagic.Implementations
{
    public interface IWorldImageProvider
    {
        SymbolsImage GetWorldImage(IImagesStorage storage);
    }
}