using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.IceObjects
{
    public class WaterIceObject : AbstractIceObject<WaterLiquidObject>
    {
        public const int WaterIceMinVolumeForEffect = 50;

        public WaterIceObject(int volume)
            : base(volume)
        {
        }

        protected override int MinVolumeForEffect => WaterIceMinVolumeForEffect;

        protected override int FreezingTemperature => WaterLiquidObject.WaterFreezingPoint;

        public override string Name => "Ice";

        protected override WaterLiquidObject CreateLiquid(int volume)
        {
            return new WaterLiquidObject(volume);
        }
    }
}