using System.IO;

namespace CodeMagic.UI.Images.Saving
{
    internal interface ISavingManager
    {
        void SaveToFile(int width, int height, SymbolsImage.Pixel[][] pixels, Stream fireStream);

        SymbolsImage LoadFromFile(Stream fileStream);
    }
}