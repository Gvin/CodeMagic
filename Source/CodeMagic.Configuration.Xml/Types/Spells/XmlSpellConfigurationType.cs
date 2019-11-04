﻿using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Spells;

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

        [XmlIgnore]
        public ISpellConfigurationCustomValue[] CustomValues =>
            CustomValuesData.ToArray<ISpellConfigurationCustomValue>();

        [XmlArray("custom")]
        [XmlArrayItem("value")]
        public XmlSpellConfigurationCustomValue[] CustomValuesData { get; set; }
    }

    [Serializable]
    public class XmlSpellConfigurationCustomValue : ISpellConfigurationCustomValue
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}