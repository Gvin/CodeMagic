using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class CaveWall : WallBase, IWorldImageProvider
    {
        private const string ImageNormal = "Wall_Cave";
        private const string ImageBottom = "Wall_Cave_Bottom";
        private const string ImageRight = "Wall_Cave_Right";
        private const string ImageBottomRight = "Wall_Cave_Bottom_Right";
        private const string ImageCorner = "Wall_Cave_Corner";

        public override string Name => "Cave Wall";

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is CaveWall || mapObject is MinableWall;
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