using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Physics;

namespace CodeMagic.Configuration.Xml.Types.Physics
{
    public class XmlTemperatureDamageConfigurationType : ITemperatureDamageConfiguration
    {
        [XmlElement("temperature-level")]
        public int TemperatureLevel { get; set; }

        [XmlElement("damage-multiplier")]
        public double DamageMultiplier { get; set; }
    }
}