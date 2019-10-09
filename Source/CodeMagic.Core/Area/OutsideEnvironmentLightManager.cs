using System;

namespace CodeMagic.Core.Area
{
    public class OutsideEnvironmentLightManager : IEnvironmentLightManager
    {
        public LightLevel GetEnvironmentLight(DateTime gameTime)
        {
            if (gameTime.Hour >= 0 && gameTime.Hour < 5)
                return LightLevel.Darkness;
            if (gameTime.Hour >= 5 && gameTime.Hour < 7)
                return LightLevel.Dusk2;
            if (gameTime.Hour >= 7 && gameTime.Hour < 8)
                return LightLevel.Dim1;
            if (gameTime.Hour >= 8 && gameTime.Hour < 9)
                return LightLevel.Dim2;
            if (gameTime.Hour >= 9 && gameTime.Hour < 16)
                return LightLevel.Medium;
            if (gameTime.Hour >= 16 && gameTime.Hour < 18)
                return LightLevel.Dim2;
            if (gameTime.Hour >= 18 && gameTime.Hour < 19)
                return LightLevel.Dim1;
            if (gameTime.Hour >= 19 && gameTime.Hour < 21)
                return LightLevel.Dusk2;
            return LightLevel.Darkness;
        }
    }
}