using CodeMagic.Core.Area.Liquids;

namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public class IceObject : AbstractIceObject
    {
        public const int WaterIceMinVolumeForEffect = 50;

        public IceObject(int volume)
            : base(volume)
        {
        }

        protected override int MinVolumeForEffect => WaterIceMinVolumeForEffect;

        protected override int FreezingTemperature => WaterLiquid.WaterFreezingPoint;

        public override string Name => "Ice";

        protected override ILiquid CreateLiquid(int volume)
        {
            return new WaterLiquid(volume);
        }
    }
}