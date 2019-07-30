using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.IceObjects
{
    public interface IWaterIceObject : IIceObject, IInjectable
    {
    }

    public class WaterIceObject : AbstractIceObject, IWaterIceObject
    {
        private const string ObjectType = "WaterIce";
        public const int WaterIceMinVolumeForEffect = 50;

        public WaterIceObject(int volume)
            : base(volume, WaterLiquidObject.LiquidType)
        {
        }

        protected override int MinVolumeForEffect => WaterIceMinVolumeForEffect;

        public override string Name => "Ice";

        public override string Type => ObjectType;

        protected override ILiquidObject CreateLiquid(int volume)
        {
            return Injector.Current.Create<IWaterLiquidObject>(volume);
        }
    }
}