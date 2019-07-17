using CodeMagic.Core.Area.Liquids;

namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public class AcidIceObject : AbstractIceObject
    {
        public const int AcidIceMinVolumeForEffect = 50;

        public AcidIceObject(int volume) 
            : base(volume)
        {
        }

        protected override int MinVolumeForEffect => AcidIceMinVolumeForEffect;

        protected override int FreezingTemperature => AcidLiquid.AcidFreezingPoint;

        public override string Name => "Acid Ice";

        protected override ILiquid CreateLiquid(int volume)
        {
            return new AcidLiquid(volume);
        }
    }
}