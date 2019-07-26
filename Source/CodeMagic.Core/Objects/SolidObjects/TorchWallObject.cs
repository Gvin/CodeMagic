using CodeMagic.Core.Area;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public class TorchWallObject : WallObject, ILightSource
    {
        public TorchWallObject(TorchWallObjectConfiguration configuration) 
            : base(configuration)
        {
            LightPower = configuration.LightPower;
        }

        public bool IsLightOn => true;

        public LightLevel LightPower { get; }
    }

    public class TorchWallObjectConfiguration : WallObjectConfiguration
    {
        public TorchWallObjectConfiguration()
        {
            LightPower = LightLevel.Medium;
        }

        public LightLevel LightPower { get; set; }
    }
}