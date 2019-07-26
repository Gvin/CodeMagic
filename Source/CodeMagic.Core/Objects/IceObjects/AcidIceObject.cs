using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.IceObjects
{
    public class AcidIceObject : AbstractIceObject
    {
        private const string ObjectType = "AcidIce";
        public const int AcidIceMinVolumeForEffect = 50;

        public AcidIceObject(int volume) 
            : base(volume, AcidLiquidObject.LiquidType)
        {
        }

        protected override int MinVolumeForEffect => AcidIceMinVolumeForEffect;

        public override string Name => "Acid Ice";

        public override string Type => ObjectType;

        protected override ILiquidObject CreateLiquid(int volume)
        {
            return MapObjectsFactory.CreateLiquidObject<AcidLiquidObject>(volume);
        }
    }
}