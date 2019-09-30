using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Buildings
{
    public class Box : StorageBase
    {
        public override string Name => "Box";
        public override int MaxWeight => 20000;
        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("Building_Box");
        }
    }
}