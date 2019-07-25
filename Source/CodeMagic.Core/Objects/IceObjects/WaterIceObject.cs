using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.IceObjects
{
    public class WaterIceObject : AbstractIceObject<WaterLiquidObject>
    {
        public const int WaterIceMinVolumeForEffect = 50;

        public WaterIceObject(int volume)
            : base(volume, WaterLiquidObject.LiquidType)
        {
        }

        protected override int MinVolumeForEffect => WaterIceMinVolumeForEffect;

        public override string Name => "Ice";

        protected override WaterLiquidObject CreateLiquid(int volume)
        {
            return MapObjectsFactory.CreateLiquidObject<WaterLiquidObject>(volume);
        }
    }
}