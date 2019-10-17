using System;
using System.Xml.Serialization;
using CodeMagic.Core.Game;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Items
{
    [Serializable]
    public class XmlElementConfiguration : IElementConfiguration
    {
        [XmlAttribute("element")]
        public Element Element { get; set; }

        [XmlElement("min-value")]
        public int MinValue { get; set; }

        [XmlElement("max-value")]
        public int MaxValue { get; set; }
    }
}