using System;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Spells
{
    [Serializable]
    public class XmlSpellConfigurationType : ISpellConfiguration
    {
        [XmlAttribute("type")]
        public string SpellType { get; set; }

        [XmlElement("mana-cost-multiplier")]
        public double ManaCostMultiplier { get; set; }

        [XmlElement("mana-cost-power")]
        public int ManaCostPower { get; set; }
    }
}