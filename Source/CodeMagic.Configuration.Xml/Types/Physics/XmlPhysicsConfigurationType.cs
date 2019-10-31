using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Physics;

namespace CodeMagic.Configuration.Xml.Types.Physics
{
    [XmlRoot("physics-configuration")]
    public class XmlPhysicsConfigurationType : IPhysicsConfiguration
    {
        [XmlIgnore]
        public ITemperatureConfiguration TemperatureConfiguration => TemperatureConfigurationData;

        [XmlElement("temperature")]
        public XmlTemperatureConfigurationType TemperatureConfigurationData { get; set; }

        [XmlIgnore]
        public IPressureConfiguration PressureConfiguration => PressureConfigurationData;

        [XmlElement("pressure")]
        public XmlPressureConfigurationType PressureConfigurationData { get; set; }

        [XmlIgnore]
        public IMagicEnergyConfiguration MagicEnergyConfiguration => MagicEnergyConfigurationData;

        [XmlElement("magic-energy")]
        public XmlMagicEnergyConfiguration MagicEnergyConfigurationData { get; set; }
    }
}