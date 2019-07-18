using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects.IceObjects;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public class AcidLiquidObject : AbstractLiquidObject<AcidIceObject>
    {
        private const string CustomValueDamageToVolumeMultiplier = "DamageToVolumeMultiplier";
        public const string LiquidType = "acid";

        public AcidLiquidObject(int volume) 
            : base(volume, LiquidType)
        {
        }

        public override string Name => "Acid";

        protected override AcidIceObject CreateIce(int volume)
        {
            return new AcidIceObject(volume);
        }

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

                var damageMultiplier = GetAcidDamageMultiplier();
                var damage = (int)Math.Ceiling(damageMultiplier * Volume);
                if (damage == 0)
                    return;

                destroyable.Damage(damage, Element.Acid);
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