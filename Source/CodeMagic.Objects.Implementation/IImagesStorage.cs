using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation
{
    public interface IImagesStorage
    {
        SymbolsImage GetImage(string name);
    }
}