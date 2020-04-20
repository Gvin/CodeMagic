using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.LiquidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.IceObjects
{
    public class WaterIce : AbstractIce, IWorldImageProvider
    {
        private const string ImageSmall = "Ice_Water_Small";
        private const string ImageMedium = "Ice_Water_Medium";
        private const string ImageBig = "Ice_Water_Big";
        private const string ObjectType = "WaterIce";
        public const int WaterIceMinVolumeForEffect = 50;

        public WaterIce(SaveData data) : base(data)
        {
        }

        public WaterIce(int volume) 
            : base(volume, WaterLiquid.LiquidType, "Ice")
        {
        }

        protected override int MinVolumeForEffect => WaterIceMinVolumeForEffect;

        public override string Type => ObjectType;

        protected override ILiquid CreateLiquid(int volume)
        {
            return new WaterLiquid(volume);
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