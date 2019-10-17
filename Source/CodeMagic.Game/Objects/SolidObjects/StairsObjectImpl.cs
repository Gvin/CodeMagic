using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class StairsObjectImpl : StairsObject, IWorldImageProvider
    {
        private const string ImageName = "Stairs_Up";

        public override string Name => "Stairs";

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageName);
        }
    }
}