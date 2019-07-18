using System.Drawing;
using CodeMagic.Core.Objects.SolidObjects;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation
{
    public class EnergyWallDrawingProcessor : IDrawingProcessor
    {
        public SymbolsImage GetImage(object @object)
        {
            return GetImage((EnergyWall) @object);
        }

        private SymbolsImage GetImage(EnergyWall wall)
        {
            var image = new SymbolsImage();

            var color = GetWallColor(wall);
            image.SetDefaultColor(color);

            image.SetSymbolMap(new []
            {
                new char?[]{ '\u250C', '-', '\u2510'},
                new char?[]{'|', '\u263C', '|'},
                new char?[]{ '\u2514', '-', '\u2518' }
            });

            image.SetPixel(1, 1, '\u263C', color, Color.LightBlue);

            return image;
        }

        private Color GetWallColor(EnergyWall wall)
        {
            if (wall.EnergyLeft < 5)
                return Color.BlueViolet;
            if (wall.EnergyLeft < 10)
                return Color.MediumVioletRed;
            if (wall.EnergyLeft < 20)
                return Color.Violet;
            if (wall.EnergyLeft < 40)
                return Color.HotPink;

            return Color.DeepPink;
        }
    }
}