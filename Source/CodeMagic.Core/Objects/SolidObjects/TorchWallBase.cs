using System.Drawing;
using CodeMagic.Core.Area;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public abstract class TorchWallBase : WallBase, ILightObject
    {
        private const LightLevel DefaultLightLevel = LightLevel.Medium;
        private static readonly Color DefaultLightColor = Color.FromArgb(255, 204, 102);

        private LightLevel LightPower => DefaultLightLevel;

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightPower, DefaultLightColor)
        };
    }
}