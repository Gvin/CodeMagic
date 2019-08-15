using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class DoorObjectImpl : DoorObject, IWorldImageProvider
    {
        private const string ImageOpenedHorizontal = "Door_Opened_Horizontal";
        private const string ImageOpenedVertical = "Door_Opened_Vertical";
        private const string ImageClosed = "Door_Closed";

        public DoorObjectImpl(bool horizontal) : base(horizontal)
        {
        }

        public override string Name => "Door";

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (Closed)
            {
                return storage.GetImage(ImageClosed);
            }

            var imageName = Horizontal ? ImageOpenedHorizontal : ImageOpenedVertical;
            return storage.GetImage(imageName);
        }
    }
}