using System;
using System.Drawing;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation
{
    public class DecorativeObjectDrawingProcessor : IDrawingProcessor
    {
        private static readonly Color RedBloodLightColor = Color.Red;
        private static readonly Color RedBloodDarkColor = Color.DarkRed;

        private static readonly Color GreenBloodLightColor = Color.GreenYellow;
        private static readonly Color GreenBloodDarkColor = Color.Green;

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

                case DecorativeObjectConfiguration.ObjectTypeGoblinBloodSmall:
                    return GetGoblinBloodSmallImage();
                case DecorativeObjectConfiguration.ObjectTypeGoblinBloodMedium:
                    return GetGoblinBloodMediumImage();
                case DecorativeObjectConfiguration.ObjectTypeGoblinBloodBig:
                    return GetGoblinBloodBigImage();

                case DecorativeObjectConfiguration.ObjectTypeWoodPieces:
                    return GetWoodPiecesImage();
                default:
                    throw new ArgumentException($"Unknown decorative object type: {@object.Type}");
            }
        }

        private static SymbolsImage GetBloodSmallImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, '.', RedBloodDarkColor);
            image.SetPixel(1, 1, ',', RedBloodLightColor);
            image.SetPixel(0, 2, '`', RedBloodLightColor);
            image.SetPixel(2, 2, '\'', RedBloodDarkColor);

            return image;
        }

        private static SymbolsImage GetBloodMediumImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, ',', RedBloodDarkColor);
            image.SetPixel(1, 1, '>', RedBloodLightColor, RedBloodDarkColor);
            image.SetPixel(2, 1, '•', RedBloodDarkColor);
            image.SetPixel(1, 2, '\'', RedBloodDarkColor);

            return image;
        }

        private static SymbolsImage GetBloodBigImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, ',', RedBloodDarkColor);
            image.SetPixel(2, 0, '·', RedBloodDarkColor);
            image.SetPixel(0, 1, '&', RedBloodLightColor, RedBloodDarkColor);
            image.SetPixel(1, 1, '#', RedBloodLightColor, RedBloodDarkColor);
            image.SetPixel(2, 1, '∂', RedBloodDarkColor);
            image.SetPixel(1, 2, '˘', RedBloodDarkColor);

            return image;
        }

        private static SymbolsImage GetGoblinBloodSmallImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, '.', GreenBloodDarkColor);
            image.SetPixel(1, 1, ',', GreenBloodLightColor);
            image.SetPixel(0, 2, '`', GreenBloodLightColor);
            image.SetPixel(2, 2, '\'', GreenBloodDarkColor);

            return image;
        }

        private static SymbolsImage GetGoblinBloodMediumImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, ',', GreenBloodDarkColor);
            image.SetPixel(1, 1, '>', GreenBloodLightColor, GreenBloodDarkColor);
            image.SetPixel(2, 1, '•', GreenBloodDarkColor);
            image.SetPixel(1, 2, '\'', GreenBloodDarkColor);

            return image;
        }

        private static SymbolsImage GetGoblinBloodBigImage()
        {
            var image = new SymbolsImage();

            image.SetPixel(0, 0, ',', GreenBloodDarkColor);
            image.SetPixel(2, 0, '·', GreenBloodDarkColor);
            image.SetPixel(0, 1, '&', GreenBloodLightColor, GreenBloodDarkColor);
            image.SetPixel(1, 1, '#', GreenBloodLightColor, GreenBloodDarkColor);
            image.SetPixel(2, 1, '∂', GreenBloodDarkColor);
            image.SetPixel(1, 2, '˘', GreenBloodDarkColor);

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