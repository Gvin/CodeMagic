using System;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Physics
{
    [Serializable]
    public class XmlMagicEnergyConfiguration : IMagicEnergyConfiguration
    {
        [XmlElement("max-value")]
        public int MaxValue { get; set; }

        [XmlElement("max-transfer-value")]
        public int MaxTransferValue { get; set; }

        [XmlElement("regeneration-value")]
        public int RegenerationValue { get; set; }

        [XmlElement("disturbance-start-level")]
        public int DisturbanceStartLevel { get; set; }

        [XmlElement("disturbance-damage-multiplier")]
        public double DisturbanceDamageMultiplier { get; set; }

        [XmlElement("disturbance-damage-start-level")]
        public int DisturbanceDamageStartLevel { get; set; }

        [XmlElement("disturbance-increment")]
        public int DisturbanceIncrement { get; set; }
    }
}