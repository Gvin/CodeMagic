using System.Drawing;
using CodeMagic.Core.Area;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public class TorchWallObject : WallObject, ILightObject
    {
        public TorchWallObject(TorchWallObjectConfiguration configuration) 
            : base(configuration)
        {
            LightPower = configuration.LightPower;
        }
        private LightLevel LightPower { get; }

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightPower, Color.FromArgb(255, 204, 102))
        };
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