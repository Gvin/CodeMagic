using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class EnergyWallImpl : EnergyWall, IWorldImageProvider
    {
        private const string ImageHighEnergy = "EnergyWall_HighEnergy";
        private const string ImageMediumEnergy = "EnergyWall_MediumEnergy";
        private const string ImageLowEnergy = "EnergyWall_LowEnergy";

        private const int MediumEnergy = 3;
        private const int HighEnergy = 10;

        public EnergyWallImpl(int lifeTime) : base(new EnergyWallConfiguration
        {
            Name = "Energy Wall",
            LifeTime = lifeTime
        })
        {
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (EnergyLeft >= HighEnergy)
                return storage.GetImage(ImageHighEnergy);
            if (EnergyLeft >= MediumEnergy)
                return storage.GetImage(ImageMediumEnergy);
            return storage.GetImage(ImageLowEnergy);
        }
    }
}