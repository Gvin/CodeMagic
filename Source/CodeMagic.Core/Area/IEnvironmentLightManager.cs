using System;

namespace CodeMagic.Core.Area
{
    public interface IEnvironmentLightManager
    {
        Light GetEnvironmentLight(DateTime gameTime);
    }
}