using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class TrapDoorObjectImpl : TrapDoorObject, IWorldImageProvider
    {
        private const string ImageName = "Decoratives_TrapDoor";

        public override string Name => "Trap Door";

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageName);
        }
    }
}