using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.IceObjects
{
    public class AcidIceImpl : AcidIceObject, IWorldImageProvider
    {
        private const string ImageSmall = "Ice_Acid_Small";
        private const string ImageMedium = "Ice_Acid_Medium";
        private const string ImageBig = "Ice_Acid_Big";

        public AcidIceImpl(int volume) 
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