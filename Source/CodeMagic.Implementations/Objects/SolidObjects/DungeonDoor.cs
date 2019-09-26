using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class DungeonDoor : DoorObject, IWorldImageProvider
    {
        private const string ImageOpenedHorizontal = "Door_Opened_Horizontal";
        private const string ImageOpenedVertical = "Door_Opened_Vertical";
        private const string ImageClosed = "Door_Closed";

        public override string Name => "Door";

        protected override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is DungeonWall || mapObject is DungeonTorchWall || mapObject is DungeonDoor;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (Closed)
            {
                return storage.GetImage(ImageClosed);
            }

            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
                return storage.GetImage(ImageOpenedVertical);

            return storage.GetImage(ImageOpenedHorizontal);
        }
    }
}