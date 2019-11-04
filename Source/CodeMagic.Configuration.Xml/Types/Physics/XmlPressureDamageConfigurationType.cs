using System;
using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Physics;

namespace CodeMagic.Configuration.Xml.Types.Physics
{
    [Serializable]
    public class XmlPressureDamageConfigurationType : IPressureDamageConfiguration
    {
        [XmlElement("pressure-level")]
        public int PressureLevel { get; set; }

        [XmlElement("damage-multiplier")]
        public double DamageMultiplier { get; set; }
    }
}