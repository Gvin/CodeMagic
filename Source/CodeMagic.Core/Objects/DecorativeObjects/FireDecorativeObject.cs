using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public interface IFireDecorativeObject : IMapObject
    {
    }

    public class FireDecorativeObject : IFireDecorativeObject, IDynamicObject
    {
        private const int MediumFireTemperature = 1200;
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

        public string Type { get; private set; }

        public string Name => "Fire";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;
        public bool BlocksEnvironment => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;

        public void Update(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            if (cell.Environment.Temperature < SmallFireTemperature)
            {
                cell.Objects.Remove(this);
                return;
            }

            Type = GetFireType(cell.Environment.Temperature);
        }

        public bool Updated { get; set; }

        public ZIndex ZIndex => ZIndex.AreaDecoration;
    }
}