using CodeMagic.UI.Images;

namespace CodeMagic.Game
{
    public interface IWorldImageProvider
    {
        SymbolsImage GetWorldImage(IImagesStorage storage);
    }
}