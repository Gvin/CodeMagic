using System;
using System.Drawing;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation
{
    public class FireDecorativeObjectDrawingProcessor : IDrawingProcessor
    {
        public SymbolsImage GetImage(object @object)
        {
            return GetImage((FireDecorativeObject)@object);
        }

        private SymbolsImage GetImage(FireDecorativeObject fire)
        {
            switch (fire.Type)
            {
                case FireDecorativeObject.ObjectTypeSmallFile:
                    return GetSmallFireImage();
                case FireDecorativeObject.ObjectTypeMediumFile:
                    return GetMediumFireImage();
                case FireDecorativeObject.ObjectTypeBigFile:
                    return GetBigFireImage();
            }
            throw new ArgumentException($"Unknown fire type: {fire.Type}");
        }

        private SymbolsImage GetSmallFireImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(1, 1, '\u2248', Color.Yellow);
            image.SetPixel(1, 0, '.', Color.Orange);
            image.SetPixel(1, 2, '\'', Color.Orange);
            image.SetPixel(0, 1, '\u2018', Color.Orange);
            image.SetPixel(2, 1, '`', Color.Orange);

            return image;
        }

        private SymbolsImage GetMediumFireImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultColor(Color.DarkOrange);

            image.SetSymbolMap(new []
            {
                new char?[]{'.', '\u201E', null},
                new char?[]{ '\u2018', '\u2591', '`'},
                new char?[]{null, '"', '`'}
            });
            image.SetColorMap(new[]
            {
                new Color?[] {Color.Orange, null, Color.Orange},
                new Color?[] {null, null, null},
                new Color?[] {Color.Orange, null, Color.Orange}
            });
            image.SetPixel(1, 1, '\u2591', Color.Red, Color.Yellow);

            return image;
        }

        private SymbolsImage GetBigFireImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(1, 1, '\u2593', Color.Red, Color.Yellow);
            image.SetPixel(1, 0, '\u2591', Color.Red, Color.Yellow);
            image.SetPixel(1, 2, '\u2591', Color.Red, Color.Yellow);
            image.SetPixel(0, 1, '\u2591', Color.Red, Color.Yellow);
            image.SetPixel(2, 1, '\u2591', Color.Red, Color.Yellow);

            image.SetPixel(0, 0, ',', Color.Orange);
            image.SetPixel(2, 0, '.', Color.Orange);
            image.SetPixel(0, 2, '\'', Color.Orange);
            image.SetPixel(2, 2, '`', Color.Orange);

            return image;
        }
    }
}
