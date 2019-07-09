using System;
using System.Drawing;
using CodeMagic.Core.Objects.SolidObjects;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation
{
    public class SolidObjectDrawingProcessor : IDrawingProcessor
    {
        public SymbolsImage GetImage(object @object)
        {
            return GetImage((SolidObject)@object);
        }

        private SymbolsImage GetImage(SolidObject @object)
        {
            switch (@object.Type)
            {
                case SolidObjectConfiguration.ObjectTypeWallStone:
                    return GetWallStoneImage();
                case SolidObjectConfiguration.ObjectTypeWallWood:
                    return GetWallWoodImage();
                case SolidObjectConfiguration.ObjectTypeHole:
                    return GetHoleImage();
                default:
                    throw new ArgumentException($"Unknown solid object type: {@object.Type}");
            }
        }

        private SymbolsImage GetWallStoneImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultValues('\u2593', Color.Gray, Color.DarkGray);

            return image;
        }

        private SymbolsImage GetWallWoodImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultValues('\u2593', Color.FromArgb(56, 35, 6), Color.SaddleBrown);

            return image;
        }

        private SymbolsImage GetHoleImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultValues(' ', Color.White, Color.Black);

            return image;
        }
    }
}