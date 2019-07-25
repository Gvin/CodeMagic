using System.Linq;
using System.Xml.Serialization;

namespace CodeMagic.UI.Images.Saving.Xml.Types
{
    [XmlRoot("symbols-image")]
    public class XmlImageType
    {
        public XmlImageType()
        {
        }

        public XmlImageType(int width, int height, SymbolsImage.Pixel[][] pixels)
        {
            Width = width;
            Height = height;

            Rows = pixels.Select(row => new XmlImageRowType(row)).ToArray();
        }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlElement("row")]
        public XmlImageRowType[] Rows { get; set; }
    }
}