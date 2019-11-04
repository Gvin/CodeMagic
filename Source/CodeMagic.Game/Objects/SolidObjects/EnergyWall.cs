using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class EnergyWall : IMapObject, IDynamicObject, ILightObject, IWorldImageProvider
    {
        private int energyLeft;
        private const string ImageHighEnergy = "EnergyWall_HighEnergy";
        private const string ImageMediumEnergy = "EnergyWall_MediumEnergy";
        private const string ImageLowEnergy = "EnergyWall_LowEnergy";

        private const int MediumEnergy = 3;
        private const int HighEnergy = 10;

        public EnergyWall(int lifeTime)
        {
            energyLeft = lifeTime;
        }

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            energyLeft--;
            if (energyLeft > 0)
                return;

            map.RemoveObject(position, this);
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Early;

        public ObjectSize Size => ObjectSize.Huge;

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(CodeSpell.DefaultLightLevel)
        };

        public bool Updated { get; set; }

        public string Name => "Energy Wall";
        public bool BlocksMovement => true;

        public bool BlocksAttack => true;

        public bool BlocksProjectiles => true;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => true;

        public ZIndex ZIndex => ZIndex.Wall;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (energyLeft >= HighEnergy)
                return storage.GetImage(ImageHighEnergy);
            if (energyLeft >= MediumEnergy)
                return storage.GetImage(ImageMediumEnergy);
            return storage.GetImage(ImageLowEnergy);
        }
    }
}