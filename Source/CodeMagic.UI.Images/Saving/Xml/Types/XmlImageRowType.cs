using System;
using System.Linq;
using System.Xml.Serialization;

namespace CodeMagic.UI.Images.Saving.Xml.Types
{
    [Serializable]
    public class XmlImageRowType
    {
        public XmlImageRowType()
        {
        }

        public XmlImageRowType(SymbolsImage.Pixel[] pixelsRow)
        {
            Pixels = pixelsRow.Select(pixel => new XmlPixelType(pixel)).ToArray();
        }

        [XmlElement("pixel")]
        public XmlPixelType[] Pixels { get; set; }
    }
}