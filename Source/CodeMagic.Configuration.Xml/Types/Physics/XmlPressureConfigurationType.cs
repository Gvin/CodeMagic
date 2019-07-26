using System;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Physics
{
    [Serializable]
    public class XmlPressureConfigurationType : IPressureConfiguration
    {
        [XmlElement("min-value")]
        public int MinValue { get; set; }

        [XmlElement("max-value")]
        public int MaxValue { get; set; }

        [XmlElement("normal-value")]
        public int NormalValue { get; set; }

        [XmlElement("normalize-speed")]
        public int NormalizeSpeed { get; set; }

        [XmlIgnore]
        public IPressureDamageConfiguration LowPressureDamageConfiguration => LowPressureDamageConfigurationData;

        [XmlElement("low-pressure-damage")]
        public XmlPressureDamageConfigurationType LowPressureDamageConfigurationData { get; set; }

        [XmlIgnore]
        public IPressureDamageConfiguration HighPressureDamageConfiguration => HighPressureDamageConfigurationData;

        [XmlElement("high-pressure-damage")]
        public XmlPressureDamageConfigurationType HighPressureDamageConfigurationData { get; set; }

        [XmlIgnore]
        public IPressureDamageConfiguration ChangePressureDamageConfiguration => ChangePressureDamageConfigurationData;

        [XmlElement("change-pressure-damage")]
        public XmlPressureDamageConfigurationType ChangePressureDamageConfigurationData { get; set; }
    }
}