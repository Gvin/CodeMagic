using System;
using System.Drawing;
using CodeMagic.Core.Objects.StationaryObjects;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation
{
    public class StationaryObjectDrawingProcessor : IDrawingProcessor
    {
        public SymbolsImage GetImage(object @object)
        {
            return GetImage((StationaryObject) @object);
        }

        private SymbolsImage GetImage(StationaryObject @object)
        {
            switch (@object.Type)
            {
                case StationaryObjectConfiguration.ObjectTypeCrates:
                    return GetCratesImage();
                default:
                    throw new ArgumentException($"Unknown stationary object type: {@object.Type}");
            }
        }

        private static SymbolsImage GetCratesImage()
        {
            var image = new SymbolsImage();
            image.SetDefaultColor(Color.SaddleBrown);

            image.SetSymbolMap(new []
            {
                new char?[]{null, '\u2580', '\u25A0'},
                new char?[]{ '\u25A0', '\u2588', '\u25A0'},
                new char?[]{null, '\u2580', null}
            });
            image.SetColorMap(new[]
            {
                new Color?[]{null, Color.FromArgb(79, 73, 66), Color.SaddleBrown},
                new Color?[]{ Color.FromArgb(89, 57, 0), Color.FromArgb(66, 46, 19), Color.SaddleBrown},
                new Color?[]{null, Color.Gray, null},
            });

            return image;
        }
    }
}