using System;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Sad.Drawing
{
    public class LightLevelManager
    {
        private const float LightLevelMultiplier = 0.2f;

        public SymbolsImage ApplyLightLevel(SymbolsImage image, LightLevel[][] lightLevelMap)
        {
            var lightLevelMultipliersMap = GetLightLevelMultipliersMap(lightLevelMap);
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
                        cell.Color = ApplyLightLevel(originalCell.Color.Value, lightLevelMultipliersMap[y][x]);
                    }

                    if (originalCell.BackgroundColor.HasValue)
                    {
                        cell.BackgroundColor = ApplyLightLevel(originalCell.BackgroundColor.Value, lightLevelMultipliersMap[y][x]);
                    }
                }
            }

            return result;
        }

        private static float[][] GetLightLevelMultipliersMap(LightLevel[][] lightLevels)
        {
            var result = new float[3][];

            result[0] = new[]
            {
                GetLightLevelPercent(lightLevels[0][0], lightLevels[1][0], lightLevels[0][1], lightLevels[1][1]),
                GetLightLevelPercent(lightLevels[0][1], lightLevels[1][1]),
                GetLightLevelPercent(lightLevels[0][2], lightLevels[1][2], lightLevels[0][1], lightLevels[1][1])
            };

            result[1] = new[]
            {
                GetLightLevelPercent(lightLevels[1][0], lightLevels[1][1]),
                GetLightLevelPercent(lightLevels[1][1]),
                GetLightLevelPercent(lightLevels[1][2], lightLevels[1][1])
            };

            result[2] = new[]
            {
                GetLightLevelPercent(lightLevels[2][0], lightLevels[1][0], lightLevels[2][1], lightLevels[1][1]),
                GetLightLevelPercent(lightLevels[2][1], lightLevels[1][1]),
                GetLightLevelPercent(lightLevels[2][2], lightLevels[1][2], lightLevels[2][1], lightLevels[1][1])
            };

            return result;
        }

        private static Color ApplyLightLevel(Color color, float lightLevelPercent)
        {
            var red = Math.Min((int)(color.R * lightLevelPercent), 255);
            var green = Math.Min((int)(color.G * lightLevelPercent), 255);
            var blue = Math.Min((int)(color.B * lightLevelPercent), 255);
            return Color.FromArgb(red, green, blue);
        }

        private static float GetLightLevelPercent(params LightLevel[] levels)
        {
            var middle = (levels.Select(l => (int) l).Sum()) / (float) levels.Length;
            return middle * LightLevelMultiplier;
        }

        private static float GetLightLevelPercent(LightLevel light)
        {
            return (int)light * LightLevelMultiplier;
        }
    }
}