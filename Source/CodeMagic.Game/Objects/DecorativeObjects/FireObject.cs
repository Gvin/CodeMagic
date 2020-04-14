using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.UI.Images;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.Objects.DecorativeObjects
{
    public class FireObject : IMapObject, IDynamicObject, ILightObject, IWorldImageProvider
    {
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

        public FireObject(int temperature)
        {
            animations = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.Random);
            this.temperature = temperature;
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

        public ObjectSize Size => ObjectSize.Huge;

        public string Name => "Fire";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;

        public bool BlocksAttack => false;

        public bool BlocksEnvironment => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;

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

        public ZIndex ZIndex => ZIndex.AreaDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }

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