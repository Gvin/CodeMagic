using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Buildings.StoneWallBuilding
{
    public class StoneWallDoor : DoorObject, IWorldImageProvider, IRoofSupport, IRoofObject
    {
        public const string BuildingKey = "stone_wall_door";

        private const string WorldImageVerticalClosed = "Building_StoneWallDoor_Vertical_Closed";
        private const string WorldImageVerticalOpened = "Building_StoneWallDoor_Vertical_Opened";
        private const string WorldImageHorizontalClosed = "Building_StoneWallDoor_Horizontal_Closed";
        private const string WorldImageHorizontalOpened = "Building_StoneWallDoor_Horizontal_Opened";

        public override string Name => "Door";

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is StoneWall || mapObject is StoneWallDoor || mapObject is StoneWallGlassWindow;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
            {
                return Closed
                    ? storage.GetImage(WorldImageVerticalClosed)
                    : storage.GetImage(WorldImageVerticalOpened);
            }

            return Closed
                ? storage.GetImage(WorldImageHorizontalClosed)
                : storage.GetImage(WorldImageHorizontalOpened);
        }
    }
}