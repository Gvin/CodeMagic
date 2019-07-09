using System;
using System.Drawing;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation
{
    public class DecorativeObjectDrawingProcessor : IDrawingProcessor
    {
        public SymbolsImage GetImage(object @object)
        {
            return GetImage((DecorativeObject)@object);
        }

        private SymbolsImage GetImage(DecorativeObject @object)
        {
            switch (@object.Type)
            {
                case DecorativeObjectConfiguration.ObjectTypeBloodSmall:
                    return GetBloodSmallImage();
                case DecorativeObjectConfiguration.ObjectTypeBloodMedium:
                    return GetBloodMediumImage();
                case DecorativeObjectConfiguration.ObjectTypeBloodBig:
                    return GetBloodBigImage();
                case DecorativeObjectConfiguration.ObjectTypeWoodPieces:
                    return GetWoodPiecesImage();
                default:
                    throw new ArgumentException($"Unknown decorative object type: {@object.Type}");
            }
        }

        private static SymbolsImage GetBloodSmallImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, '.', Color.DarkRed);
            image.SetPixel(1, 1, ',', Color.Red);
            image.SetPixel(0, 2, '`', Color.Red);
            image.SetPixel(2, 2, '\'', Color.DarkRed);

            return image;
        }

        private static SymbolsImage GetBloodMediumImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, ',', Color.DarkRed);
            image.SetPixel(1, 1, '>', Color.Red, Color.DarkRed);
            image.SetPixel(2, 1, '=', Color.DarkRed);
            image.SetPixel(1, 2, '\'', Color.DarkRed);

            return image;
        }

        private static SymbolsImage GetBloodBigImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, ',', Color.DarkRed);
            image.SetPixel(2, 0, '.', Color.DarkRed);
            image.SetPixel(0, 1, '&', Color.Red, Color.DarkRed);
            image.SetPixel(1, 1, '#', Color.Red, Color.DarkRed);
            image.SetPixel(2, 1, '%', Color.DarkRed);
            image.SetPixel(1, 2, '\'', Color.DarkRed);

            return image;
        }

        private static SymbolsImage GetWoodPiecesImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, ',', Color.SaddleBrown);
            image.SetPixel(1, 1, '/', Color.FromArgb(79, 73, 66));
            image.SetPixel(2, 1, '=', Color.FromArgb(89, 57, 0));
            image.SetPixel(1, 2, '\'', Color.FromArgb(66, 46, 19));

            return image;
        }
    }
}