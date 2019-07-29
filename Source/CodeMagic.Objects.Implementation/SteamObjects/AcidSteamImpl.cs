using System;
using CodeMagic.Core.Objects.SteamObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation.SteamObjects
{
    public class AcidSteamImpl : AcidSteamObject, IImageProvider
    {
        private const string ImageSmall = "Acid_Steam_Small";
        private const string ImageMedium = "Acid_Steam_Medium";
        private const string ImageBig = "Acid_Steam_Big";

        private const int ThicknessBig = 70;
        private const int ThicknessMedium = 30;

        private readonly AnimationsBatchManager animations;

        public AcidSteamImpl(int volume)
            : base(volume)
        {
            animations = new AnimationsBatchManager(TimeSpan.FromSeconds(1), AnimationFrameStrategy.Random);
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            var animationName = GetAnimationName();
            return animations.GetImage(storage, animationName);
        }

        private string GetAnimationName()
        {
            if (Thickness >= ThicknessBig)
                return ImageBig;
            if (Thickness >= ThicknessMedium)
                return ImageMedium;
            return ImageSmall;
        }
    }
}