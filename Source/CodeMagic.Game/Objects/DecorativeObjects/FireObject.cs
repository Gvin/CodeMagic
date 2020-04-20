using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.UI.Images;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.Objects.DecorativeObjects
{
    public class FireObject : MapObjectBase, IDynamicObject, ILightObject, IWorldImageProvider
    {
        private const string SaveKeyTemperature = "Temperature";

        private const LightLevel SmallFireLightLevel = LightLevel.Dusk2;
        private const LightLevel MediumFireLightLevel = LightLevel.Dim2;
        private const LightLevel BigFireLightLevel = LightLevel.Medium;

        private const int MediumFireTemperature = 1000;
        private const int BigFireTemperature = 1500;
        public const int SmallFireTemperature = 600;

        private int temperature;

        private const string ImageSmall = "Fire_Small";
        private const string ImageMedium = "Fire_Medium";
        private const string ImageBig = "Fire_Big";

        private readonly AnimationsBatchManager animations;

        public FireObject(SaveData data) 
            : base(data)
        {
            animations = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.Random);
            temperature = data.GetIntValue(SaveKeyTemperature);
        }

        public FireObject(int temperature)
            : base("Fire")
        {
            animations = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.Random);
            this.temperature = temperature;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyTemperature, temperature);
            return data;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var animationName = GetAnimationName();
            return animations.GetImage(storage, animationName);
        }

        private string GetAnimationName()
        {
            if (temperature >= BigFireTemperature)
                return ImageBig;
            if (temperature >= MediumFireTemperature)
                return ImageMedium;
            return ImageSmall;
        }

        public override ObjectSize Size => ObjectSize.Huge;

        public UpdateOrder UpdateOrder => UpdateOrder.Early;

        public void Update(Point position)
        {
            var cell = CurrentGame.Map.GetCell(position);
            if (cell.Temperature() < SmallFireTemperature)
            {
                CurrentGame.Map.RemoveObject(position, this);
                return;
            }

            temperature = cell.Temperature();
        }

        public bool Updated { get; set; }

        public override ZIndex ZIndex => ZIndex.AreaDecoration;

        private LightLevel LightPower
        {
            get
            {
                if (temperature >= BigFireTemperature)
                    return BigFireLightLevel;
                if (temperature >= MediumFireTemperature)
                    return MediumFireLightLevel;
                return SmallFireLightLevel;
            }
        }

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightPower)
        };
    }
}