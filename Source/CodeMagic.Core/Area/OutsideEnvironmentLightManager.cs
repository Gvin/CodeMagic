using System;
using System.Drawing;

namespace CodeMagic.Core.Area
{
    public class OutsideEnvironmentLightManager : IEnvironmentLightManager
    {
        public Light GetEnvironmentLight(DateTime gameTime)
        {
            if (gameTime.Hour >= 0 && gameTime.Hour < 5)
                return null;
            if (gameTime.Hour >= 5 && gameTime.Hour < 7)
                return new Light(Color.White, LightLevel.Dusk2, null);
            if (gameTime.Hour >= 7 && gameTime.Hour < 8)
                return new Light(Color.White, LightLevel.Dim1, null);
            if (gameTime.Hour >= 8 && gameTime.Hour < 9)
                return new Light(Color.White, LightLevel.Dim2, null);
            if (gameTime.Hour >= 9 && gameTime.Hour < 16)
                return new Light(Color.White, LightLevel.Medium, null);
            if (gameTime.Hour >= 16 && gameTime.Hour < 18)
                return new Light(Color.White, LightLevel.Dim2, null);
            if (gameTime.Hour >= 18 && gameTime.Hour < 19)
                return new Light(Color.White, LightLevel.Dim1, null);
            if (gameTime.Hour >= 19 && gameTime.Hour < 21)
                return new Light(Color.White, LightLevel.Dusk2, null);
            return null;
        }
    }
}