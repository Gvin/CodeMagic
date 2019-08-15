using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class DoorObjectImpl : DoorObject, IWorldImageProvider
    {
        private const string ImageOpened = "Door_Opened";
        private const string ImageClosed = "Door_Closed";

        public override string Name => "Door";

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (Closed)
            {
                return storage.GetImage(ImageClosed);
            }

            return storage.GetImage(ImageOpened);
        }
    }
}