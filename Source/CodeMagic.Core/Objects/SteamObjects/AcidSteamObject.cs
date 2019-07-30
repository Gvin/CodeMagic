using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.SteamObjects
{
    public interface IAcidSteamObject : ISteamObject, IInjectable
    {
    }

    public class AcidSteamObject : AbstractSteamObject, IAcidSteamObject
    {
        private const string CustomValueSteamDamageToVolumeMultiplier = "SteamDamageToVolumeMultiplier";
        private const string SteamType = "AcidSteam";

        public AcidSteamObject(int volume)
            : base(volume, AcidLiquidObject.LiquidType)
        {
        }

        public override string Name => "Acid Steam";

        public override string Type => SteamType;

        public override ISpreadingObject Separate(int volume)
        {
            Volume -= volume;
            return Injector.Current.Create<IAcidSteamObject>(volume);
        }

        protected override void UpdateSteam(IGameCore game, Point position)
        {
            base.UpdateSteam(game, position);

            var damageMultiplier = GetAcidDamageMultiplier();
            var damage = (int)Math.Ceiling(damageMultiplier * Volume);
            if (damage == 0)
                return;

            var cell = game.Map.GetCell(position);
            
            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                destroyable.Damage(damage, Element.Acid);
                game.Journal.Write(new EnvironmentDamageMessage(destroyable, damage, Element.Acid));
            }
        }

        private double GetAcidDamageMultiplier()
        {
            var stringValue = GetCustomConfigurationValue(CustomValueSteamDamageToVolumeMultiplier);
            return double.Parse(stringValue);
        }

        protected override ILiquidObject CreateLiquid(int volume)
        {
            return Injector.Current.Create<IAcidLiquidObject>(volume);
        }
    }
}