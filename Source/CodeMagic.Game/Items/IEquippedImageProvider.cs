using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public interface IEquippedImageProvider
    {
        SymbolsImage GetEquippedImage(IImagesStorage imagesStorage);

        int EquippedImageOrder { get; }
    }
}