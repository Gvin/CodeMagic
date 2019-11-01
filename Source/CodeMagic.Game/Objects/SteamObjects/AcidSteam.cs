using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.LiquidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SteamObjects
{
    public class AcidSteam : AbstractSteam, IWorldImageProvider
    {
        private const string ImageSmall = "Acid_Steam_Small";
        private const string ImageMedium = "Acid_Steam_Medium";
        private const string ImageBig = "Acid_Steam_Big";

        private const string CustomValueSteamDamageToVolumeMultiplier = "SteamDamageToVolumeMultiplier";
        private const string SteamType = "AcidSteam";

        private const int ThicknessBig = 70;
        private const int ThicknessMedium = 30;

        private readonly AnimationsBatchManager animations;

        public AcidSteam(int volume)
            : base(volume, AcidLiquid.LiquidType)
        {
            animations = new AnimationsBatchManager(TimeSpan.FromSeconds(1), AnimationFrameStrategy.Random);
        }

        public override string Name => "Acid Steam";

        public override string Type => SteamType;

        public override ISpreadingObject Separate(int volume)
        {
            Volume -= volume;
            return new AcidSteam(volume);
        }

        protected override void UpdateSteam(IAreaMap map, IJournal journal, Point position)
        {
            base.UpdateSteam(map, journal, position);

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
            var stringValue = GetCustomConfigurationValue(CustomValueSteamDamageToVolumeMultiplier);
            return double.Parse(stringValue);
        }

        protected override ILiquid CreateLiquid(int volume)
        {
            return new AcidLiquid(volume);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var animationName = GetAnimationName();
            return animations.GetImage(storage, animationName);
        }

        private string GetAnimationName()
        {
            if (Thickness >= ThicknessBig)
                return ImageBig;
            if (Thickness >= ThicknessMedium)
                return ImageMedium;
            return ImageSmall;
        }
    }
}