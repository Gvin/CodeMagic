using System.Xml.Serialization;
using CodeMagic.Core.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Physics
{
    public class XmlTemperatureDamageConfigurationType : ITemperatureDamageConfiguration
    {
        [XmlAttribute("temperature")]
        public int Temperature { get; set; }

        [XmlAttribute("damage")]
        public int Damage { get; set; }
    }
}