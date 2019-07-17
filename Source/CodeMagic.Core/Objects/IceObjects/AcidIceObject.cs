using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.IceObjects
{
    public class AcidIceObject : AbstractIceObject<AcidLiquidObject>
    {
        public const int AcidIceMinVolumeForEffect = 50;

        public AcidIceObject(int volume) 
            : base(volume)
        {
        }

        protected override int MinVolumeForEffect => AcidIceMinVolumeForEffect;

        protected override int FreezingTemperature => AcidLiquidObject.AcidFreezingPoint;

        public override string Name => "Acid Ice";

        protected override AcidLiquidObject CreateLiquid(int volume)
        {
            return new AcidLiquidObject(volume);
        }
    }
}