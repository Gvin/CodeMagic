using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public abstract class SolidWallBase : WallBase, IWorldImageProvider
    {
        protected SolidWallBase(SaveData data) : base(data)
        {
        }

        protected SolidWallBase(string name) : base(name)
        {
        }

        protected abstract string ImageNormal { get; }

        protected abstract string ImageBottom { get; }

        protected abstract string ImageRight { get; }

        protected abstract string ImageBottomRight { get; }

        protected abstract string ImageCorner { get; }

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