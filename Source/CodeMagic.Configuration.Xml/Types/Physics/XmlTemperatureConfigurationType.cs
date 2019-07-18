using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Physics
{
    public class XmlTemperatureConfigurationType : ITemperatureConfiguration
    {
        [XmlElement("normal-value")]
        public int NormalValue { get; set; }

        [XmlElement("min-value")]
        public int MinValue { get; set; }

        [XmlElement("max-value")]
        public int MaxValue { get; set; }

        [XmlElement("max-transfer-value")]
        public int MaxTransferValue { get; set; }

        [XmlElement("transfer-value-difference-multiplier")]
        public double TransferValueToDifferenceMultiplier { get; set; }

        [XmlElement("normalize-speed")]
        public int NormalizeSpeed { get; set; }

        [XmlIgnore]
        public ITemperatureDamageConfiguration[] ColdDamageConfiguration =>
            ColdDamageConfigurationData.ToArray<ITemperatureDamageConfiguration>();

        [XmlArray("cold-damage")]
        [XmlArrayItem("line")]
        public XmlTemperatureDamageConfigurationType[] ColdDamageConfigurationData { get; set; }

        [XmlIgnore]
        public ITemperatureDamageConfiguration[] HeatDamageConfiguration =>
            HeatDamageConfigurationData.ToArray<ITemperatureDamageConfiguration>();

        [XmlArray("heat-damage")]
        [XmlArrayItem("line")]
        public XmlTemperatureDamageConfigurationType[] HeatDamageConfigurationData { get; set; }
    }
}