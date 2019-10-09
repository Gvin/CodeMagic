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
    }

    public class StaticLightSource : ILightSource
    {
        public StaticLightSource(LightLevel lightPower)
        {
            LightPower = lightPower;
        }

        public LightLevel LightPower { get; }

        public bool IsLightOn => true;
    }
}