using System;

namespace CodeMagic.Core.Area
{
    public class InsideEnvironmentLightManager : IEnvironmentLightManager
    {
        public LightLevel GetEnvironmentLight(DateTime gameTime)
        {
            return LightLevel.Darkness;
        }
    }
}