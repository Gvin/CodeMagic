using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.IceObjects;
using CodeMagic.Game.Objects.SteamObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.LiquidObjects
{
    public class AcidLiquid : AbstractLiquid, IWorldImageProvider
    {
        private const string ImageSmall = "Acid_Small";
        private const string ImageMedium = "Acid_Medium";
        private const string ImageBig = "Acid_Big";
        private const string CustomValueDamageToVolumeMultiplier = "DamageToVolumeMultiplier";
        public const string LiquidType = "AcidLiquid";

        public AcidLiquid(int volume) 
            : base(volume, LiquidType)
        {
        }

        public override string Name => "Acid";

        protected override IIce CreateIce(int volume)
        {
            return new AcidIce(volume);
        }

        protected override ISteam CreateSteam(int volume)
        {
            return new AcidSteam(volume);
        }

        public override ISpreadingObject Separate(int volume)
        {
            Volume -= volume;
            return new AcidLiquid(volume);
        }

        protected override void UpdateLiquid(IAreaMap map, IJournal journal, Point position)
        {
            base.UpdateLiquid(map, journal, position);

            var damageMultiplier = GetAcidDamageMultiplier();
            var damage = (int)Math.Ceiling(damageMultiplier * Volume);
            if (damage == 0)
                return;

            var cell = map.GetCell(position);

            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                destroyable.Damage(journal, damage, Element.Acid);
                journal.Write(new EnvironmentDamageMessage(destroyable, damage, Element.Acid));
            }
        }

        private double GetAcidDamageMultiplier()
        {
            var stringValue = GetCustomConfigurationValue(CustomValueDamageToVolumeMultiplier);
            return double.Parse(stringValue);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (Volume >= Configuration.MaxVolumeBeforeSpread)
                return storage.GetImage(ImageBig);

            var halfSpread = Configuration.MaxVolumeBeforeSpread / 2;
            if (Volume >= halfSpread)
                return storage.GetImage(ImageMedium);

            return storage.GetImage(ImageSmall);
        }
    }
}