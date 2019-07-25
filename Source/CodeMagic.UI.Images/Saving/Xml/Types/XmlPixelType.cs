using System;
using System.Drawing;
using System.Xml.Serialization;

namespace CodeMagic.UI.Images.Saving.Xml.Types
{
    [Serializable]
    public class XmlPixelType
    {
        public XmlPixelType()
        {
        }

        public XmlPixelType(SymbolsImage.Pixel pixel)
        {
            Symbol = pixel.Symbol?.ToString();
            Color = pixel.Color.HasValue ? new XmlColorType(pixel.Color.Value) : null;
            BackgroundColor = pixel.BackgroundColor.HasValue ? new XmlColorType(pixel.BackgroundColor.Value) : null;
        }

        [XmlAttribute("symbol")]
        public string Symbol { get; set; }

        [XmlElement("color")]
        public XmlColorType Color { get; set; }

        [XmlElement("background-color")]
        public XmlColorType BackgroundColor { get; set; }
    }

    [Serializable]
    public class XmlColorType
    {
        public XmlColorType()
        {
        }

        public XmlColorType(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public Color GetColor()
        {
            return Color.FromArgb(R, G, B);
        }
    }
}