using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings.StoneWallBuilding
{
    public class StoneWall : WallBase, IWorldImageProvider, IRoofSupport
    {
        public const string BuildingKey = "stone_wall";

        private const string ImageNormal = "Building_StoneWall";
        private const string ImageBottom = "Building_StoneWall_Bottom";
        private const string ImageRight = "Building_StoneWall_Right";
        private const string ImageBottomRight = "Building_StoneWall_Bottom_Right";
        private const string ImageCorner = "Building_StoneWall_Corner";

        public override string Name => "Stone Wall";

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is StoneWall || mapObject is StoneWallDoor || mapObject is StoneWallGlassWindow;
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