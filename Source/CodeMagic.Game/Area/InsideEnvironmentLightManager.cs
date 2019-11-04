using System;
using CodeMagic.Core.Area;

namespace CodeMagic.Game.Area
{
    public class InsideEnvironmentLightManager : IEnvironmentLightManager
    {
        public LightLevel GetEnvironmentLight(DateTime gameTime)
        {
            return LightLevel.Darkness;
        }
    }
}