using System;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration.Monsters;

namespace CodeMagic.Configuration.Xml.Types.Monsters
{
    [Serializable]
    public class XmlMonsterImagesConfiguration : IMonsterImagesConfiguration
    {
        [XmlElement("north")]
        public string North { get; set; }

        [XmlElement("south")]
        public string South { get; set; }

        [XmlElement("west")]
        public string West { get; set; }

        [XmlElement("east")]
        public string East { get; set; }
    }
}