using System.Drawing;
using CodeMagic.Core.Area;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public abstract class TorchWallBase : WallBase, ILightObject
    {
        private const LightLevel DefaultLightLevel = LightLevel.Medium;

        private LightLevel LightPower => DefaultLightLevel;

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightPower)
        };
    }
}