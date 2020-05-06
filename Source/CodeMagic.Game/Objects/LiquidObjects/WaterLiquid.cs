﻿using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.IceObjects;
using CodeMagic.Game.Objects.SteamObjects;
using CodeMagic.Game.Statuses;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.LiquidObjects
{
    public class WaterLiquid : AbstractLiquid, IWorldImageProvider
    {
        private const string ImageSmall = "Water_Small";
        private const string ImageMedium = "Water_Medium";
        private const string ImageBig = "Water_Big";

        public const string LiquidType = "WaterLiquid";

        public WaterLiquid(int volume) 
            : base(volume, LiquidType, "Water")
        {
        }

        public WaterLiquid(SaveData data) : base(data)
        {
        }

        protected override IIce CreateIce(int volume)
        {
            return new WaterIce(volume);
        }

        protected override ISteam CreateSteam(int volume)
        {
            return new WaterSteam(volume);
        }

        public override ISpreadingObject Separate(int volume)
        {
            Volume -= volume;
            return new WaterLiquid(volume);
        }

        protected override void UpdateLiquid(Point position)
        {
            base.UpdateLiquid(position);

            var cell = CurrentGame.Map.GetCell(position);
            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                if (Volume < MinVolumeForEffect)
                    return;

                destroyable.Statuses.Remove(OnFireObjectStatus.StatusType);
                destroyable.Statuses.Remove(OilyObjectStatus.StatusType);

                destroyable.Statuses.Add(new WetObjectStatus());
            }
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