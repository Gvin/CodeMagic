using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Buildings.WoodenWallBuilding
{
    public class WoodenWallDoor : DoorObject, IWorldImageProvider
    {
        public const string BuildingKey = "wooden_wall_door";

        private const string WorldImageVerticalClosed = "Building_WoodenWallDoor_Vertical_Closed";
        private const string WorldImageVerticalOpened = "Building_WoodenWallDoor_Vertical_Opened";
        private const string WorldImageHorizontalClosed = "Building_WoodenWallDoor_Horizontal_Closed";
        private const string WorldImageHorizontalOpened = "Building_WoodenWallDoor_Horizontal_Opened";

        public override string Name => "Door";

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is WoodenWall || mapObject is WoodenWallDoor || mapObject is WoodenWallGlassWindow;
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