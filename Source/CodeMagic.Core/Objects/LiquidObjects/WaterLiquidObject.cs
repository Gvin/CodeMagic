using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public class WaterLiquidObject : AbstractLiquidObject<WaterIceObject>
    {
        public const int WaterFreezingPoint = 0;
        private const int WaterBoilingPoint = 100;
        public const int WaterMinVolumeForEffect = 50;
        private const int WaterMaxVolumeBeforeSpread = 100;
        private const int WaterMaxSpreadVolume = 3;

        private const int WaterRequiredPerDegrees = 1;
        private const int WaterSteamToPressureMultiplier = 6;

        public WaterLiquidObject(int volume) 
            : base(volume)
        {
        }

        public override string Name => "Water";

        protected override int FreezingPoint => WaterFreezingPoint;

        protected override int BoilingPoint => WaterBoilingPoint;

        protected override int MinVolumeForEffect => WaterMinVolumeForEffect;

        protected override int LiquidConsumptionPerTemperature => WaterRequiredPerDegrees;

        protected override int SteamToPressureMultiplier => WaterSteamToPressureMultiplier;

        protected override WaterIceObject CreateIce(int volume)
        {
            return new WaterIceObject(volume);
        }

        public override int MaxVolumeBeforeSpread => WaterMaxVolumeBeforeSpread;

        public override int MaxSpreadVolume => WaterMaxSpreadVolume;

        public override ILiquidObject Separate(int volume)
        {
            Volume -= volume;
            return new WaterLiquidObject(volume);
        }

        protected override void UpdateLiquid(IGameCore game, Point position)
        {
            base.UpdateLiquid(game, position);

            var cell = game.Map.GetCell(position);
            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                if (Volume < MinVolumeForEffect)
                    return;

                if (destroyable.Statuses.Contains(OnFireObjectStatus.StatusType))
                {
                    destroyable.Statuses.Remove(OnFireObjectStatus.StatusType);
                }

                destroyable.Statuses.Add(new WetObjectStatus());
            }
        }
    }
}