using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Buildings.WoodenWallBuilding
{
    public class WoodenWall : WallBase, IWorldImageProvider, IRoofSupport
    {
        public const string BuildingKey = "wooden_wall";

        private const string ImageNormal = "Building_WoodenWall";
        private const string ImageBottom = "Building_WoodenWall_Bottom";
        private const string ImageRight = "Building_WoodenWall_Right";
        private const string ImageBottomRight = "Building_WoodenWall_Bottom_Right";
        private const string ImageCorner = "Building_WoodenWall_Corner";

        public override string Name => "Wooden Wall";

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is WoodenWall || mapObject is WoodenWallDoor || mapObject is WoodenWallGlassWindow;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (!HasConnectedTile(0, 1) && !HasConnectedTile(1, 0))
            {
                return storage.GetImage(ImageBottomRight);
            }

            if (!HasConnectedTile(0, 1))
            {
                return storage.GetImage(ImageBottom);
            }

            if (!HasConnectedTile(1, 0))
            {
                return storage.GetImage(ImageRight);
            }

            if (!HasConnectedTile(1, 1))
            {
                return storage.GetImage(ImageCorner);
            }

            return storage.GetImage(ImageNormal);
        }
    }
}