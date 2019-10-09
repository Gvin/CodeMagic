using System;

namespace CodeMagic.Core.Area
{
    public interface IEnvironmentLightManager
    {
        LightLevel GetEnvironmentLight(DateTime gameTime);
    }
}