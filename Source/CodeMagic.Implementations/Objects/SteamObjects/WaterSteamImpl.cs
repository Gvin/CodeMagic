﻿using System;
using CodeMagic.Core.Objects.SteamObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SteamObjects
{
    public class WaterSteamImpl : WaterSteamObject, IImageProvider
    {
        private const string ImageSmall = "Water_Steam_Small";
        private const string ImageMedium = "Water_Steam_Medium";
        private const string ImageBig = "Water_Steam_Big";

        private const int ThicknessBig = 70;
        private const int ThicknessMedium = 30;

        private readonly AnimationsBatchManager animations;
        
        public WaterSteamImpl(int volume) 
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