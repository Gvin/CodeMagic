using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.SteamObjects
{
    public interface IWaterSteamObject : ISteamObject, IInjectable
    {
    }

    public class WaterSteamObject : AbstractSteamObject, IWaterSteamObject
    {
        private const string SteamType = "WaterSteam";

        public WaterSteamObject(int volume) 
            : base(volume, WaterLiquidObject.LiquidType)
        {
        }

        public override string Name => "Water Steam";

        public override string Type => SteamType;

        public override ISpreadingObject Separate(int volume)
        {
            Volume -= volume;
            return Injector.Current.Create<IWaterSteamObject>(volume);
        }

        protected override ILiquidObject CreateLiquid(int volume)
        {
            return Injector.Current.Create<IWaterLiquidObject>(volume);
        }
    }
}