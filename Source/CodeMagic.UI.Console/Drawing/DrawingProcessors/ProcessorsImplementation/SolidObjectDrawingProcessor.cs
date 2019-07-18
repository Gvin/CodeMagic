using System;
using System.Drawing;
using CodeMagic.Core.Common;
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
                    return GetWallStoneImage(@object);
                case SolidObjectConfiguration.ObjectTypeWallWood:
                    return GetWallWoodImage();
                case SolidObjectConfiguration.ObjectTypeHole:
                    return GetHoleImage();
                default:
                    throw new ArgumentException($"Unknown solid object type: {@object.Type}");
            }
        }

        private SymbolsImage GetWallStoneImage(SolidObject wall)
        {
            var image = new SymbolsImage();

            image.SetDefaultValues('\u2593', Color.Gray, Color.DarkGray);

            if (!wall.HasConnectedTile(Direction.Down))
            {
                image.SetPixel(0, 2, '\u2593', Color.FromArgb(89, 89, 89), Color.FromArgb(153, 153, 153));
                image.SetPixel(1, 2, '\u2593', Color.FromArgb(89, 89, 89), Color.FromArgb(153, 153, 153));
                image.SetPixel(2, 2, '\u2593', Color.FromArgb(89, 89, 89), Color.FromArgb(153, 153, 153));
            }

            if (!wall.HasConnectedTile(Direction.Right))
            {
                image.SetPixel(2, 0, '\u2593', Color.FromArgb(89, 89, 89), Color.FromArgb(153, 153, 153));
                image.SetPixel(2, 1, '\u2593', Color.FromArgb(89, 89, 89), Color.FromArgb(153, 153, 153));
                image.SetPixel(2, 2, '\u2593', Color.FromArgb(89, 89, 89), Color.FromArgb(153, 153, 153));
            }

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