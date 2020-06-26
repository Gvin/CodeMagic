using System;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Items
{
    [Serializable]
    public class XmlIntervalConfiguration : IIntervalConfiguration
    {
        [XmlAttribute("min")]
        public int Min { get; set; }

        [XmlAttribute("max")]
        public int Max { get; set; }
    }
}