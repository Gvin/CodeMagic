using CodeMagic.Game.Objects.LiquidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.IceObjects
{
    public class AcidIce : AbstractIce, IWorldImageProvider
    {
        private const string ImageSmall = "Ice_Acid_Small";
        private const string ImageMedium = "Ice_Acid_Medium";
        private const string ImageBig = "Ice_Acid_Big";
        private const string ObjectType = "AcidIce";
        public const int AcidIceMinVolumeForEffect = 50;

        public AcidIce(int volume) 
            : base(volume, AcidLiquid.LiquidType)
        {
        }

        protected override int MinVolumeForEffect => AcidIceMinVolumeForEffect;

        public override string Name => "Acid Ice";

        public override string Type => ObjectType;

        protected override ILiquid CreateLiquid(int volume)
        {
            return new AcidLiquid(volume);
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