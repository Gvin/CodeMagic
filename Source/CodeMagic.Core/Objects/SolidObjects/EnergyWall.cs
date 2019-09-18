using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Spells;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public interface IEnergyWall : IWallObject, IInjectable
    {
    }

    public class EnergyWall : IEnergyWall, IDynamicObject, ILightObject
    {
        public EnergyWall(EnergyWallConfiguration configuration)
        {
            Name = configuration.Name;
            EnergyLeft = configuration.LifeTime;
        }

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            EnergyLeft--;
            if (EnergyLeft > 0)
                return;

            var cell = map.GetCell(position);
            map.RemoveObject(position, this);
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Early;

        public ObjectSize Size => ObjectSize.Huge;

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(CodeSpell.DefaultLightLevel, Color.Red)
        };

        public int EnergyLeft { get; private set; }

        public bool Updated { get; set; }

        public string Name { get; }
        public bool BlocksMovement => true;
        public bool BlocksProjectiles => true;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => true;

        public ZIndex ZIndex => ZIndex.Wall;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }
    }

    public class EnergyWallConfiguration
    {
        public string Name { get; set; }

        public int LifeTime { get; set; }
    }
}