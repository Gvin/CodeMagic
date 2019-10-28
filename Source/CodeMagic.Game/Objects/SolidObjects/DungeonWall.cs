using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class DungeonWall : WallBase, IWorldImageProvider
    {
        private const string ImageNormal = "Wall_Dungeon";
        private const string ImageBottom = "Wall_Dungeon_Bottom";
        private const string ImageRight = "Wall_Dungeon_Right";
        private const string ImageBottomRight = "Wall_Dungeon_Bottom_Right";
        private const string ImageCorner = "Wall_Dungeon_Corner";

        public override string Name => "Dungeon Wall";

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is DungeonWall || mapObject is DungeonTorchWall || mapObject is DungeonDoor;
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