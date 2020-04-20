using System;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.LiquidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SteamObjects
{
    public class WaterSteam : AbstractSteam, IWorldImageProvider
    {
        private const string ImageSmall = "Water_Steam_Small";
        private const string ImageMedium = "Water_Steam_Medium";
        private const string ImageBig = "Water_Steam_Big";
        private const string SteamType = "WaterSteam";

        private const int ThicknessBig = 70;
        private const int ThicknessMedium = 30;

        private readonly AnimationsBatchManager animations;

        public WaterSteam(SaveData data) : base(data)
        {
            animations = new AnimationsBatchManager(TimeSpan.FromSeconds(1), AnimationFrameStrategy.Random);
        }

        public WaterSteam(int volume) 
            : base(volume, WaterLiquid.LiquidType, "Water Steam")
        {
            animations = new AnimationsBatchManager(TimeSpan.FromSeconds(1), AnimationFrameStrategy.Random);
        }

        public override string Type => SteamType;

        public override ISpreadingObject Separate(int volume)
        {
            Volume -= volume;
            return new WaterSteam(volume);
        }

        protected override ILiquid CreateLiquid(int volume)
        {
            return new WaterLiquid(volume);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
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