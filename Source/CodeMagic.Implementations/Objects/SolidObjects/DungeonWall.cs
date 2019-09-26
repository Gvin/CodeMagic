using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class DungeonWall : WallBase, IWorldImageProvider
    {
        private const string ImageNormal = "Wall_Stone";
        private const string ImageBottom = "Wall_Stone_Bottom";
        private const string ImageRight = "Wall_Stone_Right";
        private const string ImageBottomRight = "Wall_Stone_Bottom_Right";
        private const string ImageCorner = "Wall_Stone_Corner";

        public override string Name => "Stone Wall";

        protected override bool CanConnectTo(IMapObject mapObject)
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