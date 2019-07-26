using CodeMagic.Core.Area;

namespace CodeMagic.Core.Objects
{
    public interface ILightSource
    {
        bool IsLightOn { get; }

        LightLevel LightPower { get; }
    }
}