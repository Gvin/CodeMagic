using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.IceObjects
{
    public class WaterIceImpl : WaterIceObject, IImageProvider
    {
        private const string ImageSmall = "Ice_Water_Small";
        private const string ImageMedium = "Ice_Water_Medium";
        private const string ImageBig = "Ice_Water_Big";

        public WaterIceImpl(int volume) 
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