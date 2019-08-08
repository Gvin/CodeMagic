using CodeMagic.UI.Images;

namespace CodeMagic.Implementations
{
    public interface IImagesStorage
    {
        SymbolsImage GetImage(string name);

        SymbolsImage[] GetAnimation(string name);
    }
}