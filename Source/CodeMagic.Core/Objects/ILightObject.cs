using System.Drawing;
using CodeMagic.Core.Area;

namespace CodeMagic.Core.Objects
{
    public interface ILightObject
    {
        ILightSource[] LightSources { get; }
    }

    public interface ILightSource
    {
        bool IsLightOn { get; }

        LightLevel LightPower { get; }

        Color LightColor { get; }
    }

    public class StaticLightSource : ILightSource
    {
        public StaticLightSource(LightLevel lightPower, Color lightColor)
        {
            LightPower = lightPower;
            LightColor = lightColor;
        }

        public LightLevel LightPower { get; }

        public Color LightColor { get; }

        public bool IsLightOn => true;
    }
}