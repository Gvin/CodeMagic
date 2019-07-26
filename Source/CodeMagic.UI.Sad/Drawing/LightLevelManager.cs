using System;
using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Sad.Drawing
{
    public class LightLevelManager
    {
        public SymbolsImage ApplyLightLevel(SymbolsImage image, LightLevel lightLevel)
        {
            var result = new SymbolsImage(image.Width, image.Height);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var originalCell = image[x, y];
                    var cell = result[x, y];

                    cell.Symbol = originalCell.Symbol;

                    if (originalCell.Color.HasValue)
                    {
                        cell.Color = ApplyLightLevel(originalCell.Color.Value, lightLevel);
                    }

                    if (originalCell.BackgroundColor.HasValue)
                    {
                        cell.BackgroundColor = ApplyLightLevel(originalCell.BackgroundColor.Value, lightLevel);
                    }
                }
            }

            return result;
        }

        private static Color ApplyLightLevel(Color color, LightLevel lightLevel)
        {
            var lightLevelPercent = GetLightLevelPercent(lightLevel);
            var red = Math.Min((int)(color.R * lightLevelPercent), 255);
            var green = Math.Min((int)(color.G * lightLevelPercent), 255);
            var blue = Math.Min((int)(color.B * lightLevelPercent), 255);
            return Color.FromArgb(red, green, blue);
        }

        private static float GetLightLevelPercent(LightLevel light)
        {
            return (int)light * 0.2f;
        }
    }
}