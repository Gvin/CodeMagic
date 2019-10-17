using System;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.DecorativeObjects
{
    public class FireObjectImpl : FireDecorativeObject, IWorldImageProvider
    {
        private const string ImageSmall = "Fire_Small";
        private const string ImageMedium = "Fire_Medium";
        private const string ImageBig = "Fire_Big";

        private readonly AnimationsBatchManager animations;

        public FireObjectImpl(int temperature) : base(temperature)
        {
            animations = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.Random);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            switch (Type)
            {
                case ObjectTypeSmallFire:
                    return animations.GetImage(storage, ImageSmall);
                case ObjectTypeMediumFire:
                    return animations.GetImage(storage, ImageMedium);
                case ObjectTypeBigFire:
                    return animations.GetImage(storage, ImageBig);
                default:
                    throw new ApplicationException($"Unknown fire type: {Type}");
            }
        }
    }
}