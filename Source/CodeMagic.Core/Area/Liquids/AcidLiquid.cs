using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.Core.Area.Liquids
{
    public class AcidLiquid : AbstractLiquid
    {
        public const int AcidFreezingPoint = -10;
        private const int AcidBoilingPoint = 337;

        private const int AcidMaxVolume = 100;
        private const int AcidMaxSpreadVolume = 3;

        private const int AcidRequiredPerDegrees = 1;
        private const int SteamToPressureMultiplier = 6;

        public const int MinVolumeForEffect = 50;

        private const double AcidDamageToVolumeMultiplier = 0.3d;

        public AcidLiquid(int volume) : base(volume)
        {
        }

        protected override int FreezingPoint => AcidFreezingPoint;

        protected override int BoilingPoint => AcidBoilingPoint;

        public override int MaxVolume => AcidMaxVolume;

        public override int MaxSpreadVolume => AcidMaxSpreadVolume;

        public override ILiquid Separate(int separateVolume)
        {
            Volume -= separateVolume;
            return new AcidLiquid(separateVolume);
        }

        protected override void ProcessBoiling(AreaMapCell cell)
        {
            var excessTemperature = cell.Environment.Temperature - AcidBoilingPoint;
            var steamVolume = Math.Min(excessTemperature * AcidRequiredPerDegrees, Volume);
            var heatLoss = steamVolume / AcidRequiredPerDegrees;

            cell.Environment.Temperature -= heatLoss;
            cell.Environment.Pressure += steamVolume * SteamToPressureMultiplier;
            Volume -= steamVolume;
        }

        protected override void ProcessFreezing(AreaMapCell cell)
        {
            var ice = cell.Objects.OfType<AcidIceObject>().FirstOrDefault();
            if (ice == null)
            {
                ice = new AcidIceObject(0);
                cell.Objects.Add(ice);
            }

            ice.Volume += Volume;
            Volume = 0;
        }

        public override void ApplyEffect(IDestroyableObject destroyable, Journal journal)
        {
            if (Volume < MinVolumeForEffect)
                return;

            var damage = (int)Math.Ceiling(AcidDamageToVolumeMultiplier * Volume);
            if (damage == 0)
                return;

            destroyable.Damage(damage, Element.Acid);
            journal.Write(new EnvironmentDamageMessage(destroyable, damage, Element.Acid));
        }
    }
}