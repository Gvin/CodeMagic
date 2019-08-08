using System;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class TorchWallImpl : TorchWallObject, IImageProvider
    {
        private const string ImageNormal = "Wall_{0}";
        private const string ImageBottom = "Wall_{0}_Bottom_Torch";
        private const string ImageRight = "Wall_{0}_Right_Torch";
        private const string ImageBottomRight = "Wall_{0}_Bottom_Right_Torch";
        private const string ImageCorner = "Wall_{0}_Corner";

        private readonly AnimationsBatchManager animationsManager;

        public TorchWallImpl(TorchWallObjectConfiguration configuration) 
            : base(configuration)
        {
            animationsManager = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.Random);
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            if (!HasConnectedTile(0, 1) && !HasConnectedTile(1, 0))
            {
                return animationsManager.GetImage(storage, ParseImageTemplate(ImageBottomRight));
            }

            if (!HasConnectedTile(0, 1))
            {
                return animationsManager.GetImage(storage, ParseImageTemplate(ImageBottom));
            }

            if (!HasConnectedTile(1, 0))
            {
                return animationsManager.GetImage(storage, ParseImageTemplate(ImageRight));
            }

            if (!HasConnectedTile(1, 1))
            {
                return GetImage(storage, ImageCorner);
            }

            return GetImage(storage, ImageNormal);
        }

        private string ParseImageTemplate(string template)
        {
            return string.Format(template, Type);
        }

        private SymbolsImage GetImage(IImagesStorage storage, string template)
        {
            var key = ParseImageTemplate(template);
            return storage.GetImage(key);
        }
    }
}