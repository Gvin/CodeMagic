using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation.LiquidObjects
{
    public class OilLiquidImpl : OilLiquidObject, IImageProvider
    {
        private const string ImageSmall = "Oil_Small";
        private const string ImageMedium = "Oil_Medium";
        private const string ImageBig = "Oil_Big";

        public OilLiquidImpl(int volume) 
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