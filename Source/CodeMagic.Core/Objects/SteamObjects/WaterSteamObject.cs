using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.SteamObjects
{
    public class WaterSteamObject : AbstractSteamObject
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
            return MapObjectsFactory.CreateSteam<WaterSteamObject>(volume);
        }

        protected override ILiquidObject CreateLiquid(int volume)
        {
            return MapObjectsFactory.CreateLiquidObject<WaterLiquidObject>(volume);
        }
    }
}