﻿using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation.LiquidObjects
{
    public class WaterLiquidImpl : WaterLiquidObject, IImageProvider
    {
        private const string ImageSmall = "Water_Small";
        private const string ImageMedium = "Water_Medium";
        private const string ImageBig = "Water_Big";

        public WaterLiquidImpl(int volume) 
            : base(volume)
        {
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            if (Volume >= Configuration.MaxVolumeBeforeSpread)
                return storage.GetImage(ImageBig);

            var halfSpread = Configuration.MaxVolumeBeforeSpread / 2;
            if (Volume >= halfSpread)
                return storage.GetImage(ImageMedium);

            return storage.GetImage(ImageSmall);
        }
    }
}