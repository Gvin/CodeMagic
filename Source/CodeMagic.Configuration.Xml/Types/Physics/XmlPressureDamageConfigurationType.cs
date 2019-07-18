using System;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Physics
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