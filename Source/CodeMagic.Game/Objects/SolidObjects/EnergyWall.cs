using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class EnergyWall : MapObjectBase, IDynamicObject, ILightObject, IWorldImageProvider
    {
        private const string SaveKeyEnergyLeft = "EnergyLeft";

        private const string ImageHighEnergy = "EnergyWall_HighEnergy";
        private const string ImageMediumEnergy = "EnergyWall_MediumEnergy";
        private const string ImageLowEnergy = "EnergyWall_LowEnergy";

        private const int MediumEnergy = 3;
        private const int HighEnergy = 10;

        private int energyLeft;

        public EnergyWall(SaveData data) : base(data)
        {
            energyLeft = data.GetIntValue(SaveKeyEnergyLeft);
        }

        public EnergyWall(int lifeTime)
            : base("Energy Wall")
        {
            energyLeft = lifeTime;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyEnergyLeft, energyLeft);
            return data;
        }

        public void Update(Point position)
        {
            energyLeft--;
            if (energyLeft > 0)
                return;

            CurrentGame.Map.RemoveObject(position, this);
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Early;

        public override ObjectSize Size => ObjectSize.Huge;

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(CodeSpell.DefaultLightLevel)
        };

        public bool Updated { get; set; }

        public override bool BlocksMovement => true;

        public override bool BlocksAttack => true;

        public override bool BlocksProjectiles => true;

        public override bool BlocksEnvironment => true;

        public override ZIndex ZIndex => ZIndex.Wall;

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