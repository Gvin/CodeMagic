using CodeMagic.UI.Images;

namespace CodeMagic.Game
{
    public interface IImagesStorage
    {
        SymbolsImage GetImage(string name);

        SymbolsImage[] GetAnimation(string name);
    }
}