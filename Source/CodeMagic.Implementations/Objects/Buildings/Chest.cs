using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Buildings
{
    public class Chest : StorageBase
    {
        public override string Name => "Chest";

        public override int MaxWeight => 50000;

        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("Building_Chest");
        }
    }
}