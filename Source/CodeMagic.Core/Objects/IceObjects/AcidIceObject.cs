using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.IceObjects
{
    public interface IAcidIceObject : IIceObject, IInjectable
    {
    }

    public class AcidIceObject : AbstractIceObject, IAcidIceObject
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
            return Injector.Current.Create<IAcidLiquidObject>(volume);
        }
    }
}