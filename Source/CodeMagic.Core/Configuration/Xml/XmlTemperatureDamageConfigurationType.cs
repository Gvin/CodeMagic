using System.Xml.Serialization;

namespace CodeMagic.Core.Configuration.Xml
{
    public class XmlTemperatureDamageConfigurationType : ITemperatureDamageConfiguration
    {
        [XmlAttribute("temperature")]
        public int Temperature { get; set; }

        [XmlAttribute("damage")]
        public int Damage { get; set; }
    }
}