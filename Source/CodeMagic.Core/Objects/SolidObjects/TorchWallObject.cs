using System.Drawing;
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

        public Color LightColor => Color.FromArgb(255, 204, 102);
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