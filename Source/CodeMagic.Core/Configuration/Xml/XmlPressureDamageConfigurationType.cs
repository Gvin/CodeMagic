using System;
using System.Xml.Serialization;

namespace CodeMagic.Core.Configuration.Xml
{
    [Serializable]
    public class XmlPressureDamageConfigurationType : IPressureDamageConfiguration
    {
        [XmlAttribute("pressure")]
        public int Pressure { get; set; }

        [XmlAttribute("damage")]
        public int Damage { get; set; }
    }
}