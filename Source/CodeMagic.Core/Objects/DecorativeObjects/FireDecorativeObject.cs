using System;
using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Injection;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public interface IFireDecorativeObject : IMapObject, IInjectable
    {
    }

    public class FireDecorativeObject : IFireDecorativeObject, IDynamicObject, ILightObject
    {
        private const LightLevel SmallFireLightLevel = LightLevel.Dusk2;
        private const LightLevel MediumFireLightLevel = LightLevel.Dim2;
        private const LightLevel BigFireLightLevel = LightLevel.Medium;

        private const int MediumFireTemperature = 1000;
        private const int BigFireTemperature = 1500;
        public const int SmallFireTemperature = 600;

        public const string ObjectTypeSmallFire = "SmallFire";
        public const string ObjectTypeMediumFire = "MediumFire";
        public const string ObjectTypeBigFire = "BigFire";

        public FireDecorativeObject(int temperature)
        {
            Type = GetFireType(temperature);
        }

        private string GetFireType(int temperature)
        {
            if (temperature >= BigFireTemperature)
                return ObjectTypeBigFire;
            if (temperature >= MediumFireTemperature)
                return ObjectTypeMediumFire;
            return ObjectTypeSmallFire;
        }

        public ObjectSize Size => ObjectSize.Huge;

        public string Type { get; private set; }

        public string Name => "Fire";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;

        public bool BlocksAttack => false;

        public bool BlocksEnvironment => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;

        public UpdateOrder UpdateOrder => UpdateOrder.Early;

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            var cell = map.GetCell(position);
            if (cell.Temperature < SmallFireTemperature)
            {
                map.RemoveObject(position, this);
                return;
            }

            Type = GetFireType(cell.Temperature);
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
                switch (Type)
                {
                    case ObjectTypeSmallFire:
                        return SmallFireLightLevel;
                    case ObjectTypeMediumFire:
                        return MediumFireLightLevel;
                    case ObjectTypeBigFire:
                        return BigFireLightLevel;
                    default:
                        throw new ApplicationException($"Unknown fire object type: {Type}");
                }
            }
        }

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightPower, Color.DarkOrange)
        };
    }
}