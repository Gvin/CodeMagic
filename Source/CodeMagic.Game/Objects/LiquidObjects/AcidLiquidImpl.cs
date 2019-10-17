using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.LiquidObjects
{
    public class AcidLiquidImpl : AcidLiquidObject, IWorldImageProvider
    {
        private const string ImageSmall = "Acid_Small";
        private const string ImageMedium = "Acid_Medium";
        private const string ImageBig = "Acid_Big";

        public AcidLiquidImpl(int volume) 
            : base(volume)
        {
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
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