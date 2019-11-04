﻿using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Physics;

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

        [XmlElement("normalize-speed-inside")]
        public int NormalizeSpeedInside { get; set; }

        [XmlElement("normalize-speed-outside")]
        public int NormalizeSpeedOutside { get; set; }

        [XmlIgnore]
        public ITemperatureDamageConfiguration ColdDamageConfiguration =>ColdDamageConfigurationData;

        [XmlElement("cold-damage")]
        public XmlTemperatureDamageConfigurationType ColdDamageConfigurationData { get; set; }

        [XmlIgnore]
        public ITemperatureDamageConfiguration HeatDamageConfiguration => HeatDamageConfigurationData;

        [XmlElement("heat-damage")]
        public XmlTemperatureDamageConfigurationType HeatDamageConfigurationData { get; set; }
    }
}