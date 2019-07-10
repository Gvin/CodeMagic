using CodeMagic.Core.Area;
using CodeMagic.Core.Area.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public class IceObject : IMapObject, IDynamicObject
    {
        public const int MinVolumeForEffect = 50;

        public IceObject(int volume)
        {
            Volume = volume;
        }

        public int Volume { get; set; }

        public string Name => "Ice";

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public void Update(IAreaMap map, Point position, Journal journal)
        {
            var cell = map.GetCell(position);
            if (cell.Environment.Temperature > WaterLiquid.FreezingPoint)
            {
                cell.Liquids.AddLiquid(new WaterLiquid(Volume));
                cell.Objects.Remove(this);
            }
        }

        public bool Updated { get; set; }
    }
}