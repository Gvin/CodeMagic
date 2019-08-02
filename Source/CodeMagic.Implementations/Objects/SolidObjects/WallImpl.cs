using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class WallImpl : WallObject, IImageProvider
    {
        private const string ImageNormal = "Wall_{0}";
        private const string ImageBottom = "Wall_{0}_Bottom";
        private const string ImageRight = "Wall_{0}_Right";
        private const string ImageBottomRight = "Wall_{0}_Bottom_Right";
        private const string ImageCorner = "Wall_{0}_Corner";

        public WallImpl(WallObjectConfiguration configuration) 
            : base(configuration)
        {
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            if (!HasConnectedTile(0, 1) && !HasConnectedTile(1, 0))
            {
                return GetImage(storage, ImageBottomRight);
            }

            if (!HasConnectedTile(0, 1))
            {
                return GetImage(storage, ImageBottom);
            }

            if (!HasConnectedTile(1, 0))
            {
                return GetImage(storage, ImageRight);
            }

            if (!HasConnectedTile(1, 1))
            {
                return GetImage(storage, ImageCorner);
            }

            return GetImage(storage, ImageNormal);
        }

        private SymbolsImage GetImage(IImagesStorage storage, string template)
        {
            var key = string.Format(template, Type);
            return storage.GetImage(key);
        }
    }
}