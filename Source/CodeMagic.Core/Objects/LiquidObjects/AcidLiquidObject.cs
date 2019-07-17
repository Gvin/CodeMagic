using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects.IceObjects;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public class AcidLiquidObject : AbstractLiquidObject<AcidIceObject>
    {
        public const int AcidFreezingPoint = -10;
        private const int AcidBoilingPoint = 337;

        private const int AcidMaxVolumeBeforeSpread = 100;
        private const int AcidMaxSpreadVolume = 3;

        private const int AcidRequiredPerDegrees = 1;
        private const int AcidSteamToPressureMultiplier = 6;

        public const int AcidMinVolumeForEffect = 50;

        private const double AcidDamageToVolumeMultiplier = 0.3d;

        public AcidLiquidObject(int volume) 
            : base(volume)
        {
        }

        public override string Name => "Acid";

        protected override int FreezingPoint => AcidFreezingPoint;

        protected override int BoilingPoint => AcidBoilingPoint;

        protected override int MinVolumeForEffect => AcidMinVolumeForEffect;

        protected override int LiquidConsumptionPerTemperature => AcidRequiredPerDegrees;

        protected override int SteamToPressureMultiplier => AcidSteamToPressureMultiplier;

        protected override AcidIceObject CreateIce(int volume)
        {
            return new AcidIceObject(volume);
        }

        public override int MaxVolumeBeforeSpread => AcidMaxVolumeBeforeSpread;

        public override int MaxSpreadVolume => AcidMaxSpreadVolume;

        public override ILiquidObject Separate(int volume)
        {
            Volume -= volume;
            return new AcidLiquidObject(volume);
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

                var damage = (int)Math.Ceiling(AcidDamageToVolumeMultiplier * Volume);
                if (damage == 0)
                    return;

                destroyable.Damage(damage, Element.Acid);
                game.Journal.Write(new EnvironmentDamageMessage(destroyable, damage, Element.Acid));
            }
        }
    }
}