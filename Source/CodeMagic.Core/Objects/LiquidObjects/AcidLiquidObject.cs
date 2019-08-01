using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Objects.SteamObjects;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public interface IAcidLiquidObject : ILiquidObject, IInjectable
    {
    }

    public class AcidLiquidObject : AbstractLiquidObject, IAcidLiquidObject
    {
        private const string CustomValueDamageToVolumeMultiplier = "DamageToVolumeMultiplier";
        public const string LiquidType = "AcidLiquid";

        public AcidLiquidObject(int volume) 
            : base(volume, LiquidType)
        {
        }

        public override string Name => "Acid";

        protected override IIceObject CreateIce(int volume)
        {
            return Injector.Current.Create<IAcidIceObject>(volume);
        }

        protected override ISteamObject CreateSteam(int volume)
        {
            return Injector.Current.Create<IAcidSteamObject>(volume);
        }

        public override ISpreadingObject Separate(int volume)
        {
            Volume -= volume;
            return Injector.Current.Create<IAcidLiquidObject>(volume);
        }

        protected override void UpdateLiquid(IGameCore game, Point position)
        {
            base.UpdateLiquid(game, position);

            var damageMultiplier = GetAcidDamageMultiplier();
            var damage = (int)Math.Ceiling(damageMultiplier * Volume);
            if (damage == 0)
                return;

            var cell = game.Map.GetCell(position);

            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                destroyable.Damage(game.Journal, damage, Element.Acid);
                game.Journal.Write(new EnvironmentDamageMessage(destroyable, damage, Element.Acid));
            }
        }

        private double GetAcidDamageMultiplier()
        {
            var stringValue = GetCustomConfigurationValue(CustomValueDamageToVolumeMultiplier);
            return double.Parse(stringValue);
        }
    }
}