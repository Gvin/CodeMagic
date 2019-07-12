using System;
using System.Linq;
using System.Xml.Serialization;

namespace CodeMagic.Core.Configuration.Xml
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
        public IPressureDamageConfiguration[] LowPressureDamageConfiguration =>
            LowPressureDamageConfigurationData.ToArray<IPressureDamageConfiguration>();

        [XmlArray("low-pressure-damage")]
        [XmlArrayItem("line")]
        public XmlPressureDamageConfigurationType[] LowPressureDamageConfigurationData { get; set; }

        [XmlIgnore]
        public IPressureDamageConfiguration[] HighPressureDamageConfiguration =>
            HighPressureDamageConfigurationData.ToArray<IPressureDamageConfiguration>();

        [XmlArray("high-pressure-damage")]
        [XmlArrayItem("line")]
        public XmlPressureDamageConfigurationType[] HighPressureDamageConfigurationData { get; set; }

        [XmlIgnore]
        public IPressureDamageConfiguration[] ChangePressureDamageConfiguration =>
            ChangePressureDamageConfigurationData.ToArray<IPressureDamageConfiguration>();

        [XmlArray("change-pressure-damage")]
        [XmlArrayItem("line")]
        public XmlPressureDamageConfigurationType[] ChangePressureDamageConfigurationData { get; set; }
    }
}