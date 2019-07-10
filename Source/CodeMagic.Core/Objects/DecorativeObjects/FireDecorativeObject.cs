using CodeMagic.Core.Area;
using CodeMagic.Core.Area.EnvironmentData;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public class FireDecorativeObject : IMapObject, IDynamicObject
    {
        public const string ObjectTypeSmallFile = "SmallFire";
        public const string ObjectTypeMediumFile = "MediumFire";
        public const string ObjectTypeBigFile = "BigFire";

        public FireDecorativeObject(int temperature)
        {
            Type = GetFireType(temperature);
        }

        private string GetFireType(int temperature)
        {
            if (temperature >= Temperature.MetalMeltTemperature)
                return ObjectTypeBigFile;
            if (temperature >= Temperature.StoneMeltTemperature)
                return ObjectTypeMediumFile;
            return ObjectTypeSmallFile;
        }

        public string Type { get; private set; }

        public string Name => "Fire";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;
        public bool BlocksEnvironment => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;
        public void Update(IAreaMap map, Point position, Journal journal)
        {
            var cell = map.GetCell(position);
            if (cell.Environment.Temperature < Temperature.WoodBurnTemperature)
            {
                cell.Objects.Remove(this);
                return;
            }

            Type = GetFireType(cell.Environment.Temperature);
        }

        public bool Updated { get; set; }
    }
}