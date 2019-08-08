using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Configuration.Xml.Types.Items.Bonuses
{
    [Serializable]
    public class XmlBonusConfiguration : IBonusConfiguration
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlIgnore]
        public Dictionary<string, string> Values => ValuesData.ToDictionary(value => value.Key, value => value.Value);

        [XmlElement("value")]
        public XmlBonusValue[] ValuesData { get; set; }

        public class XmlBonusValue
        {
            [XmlAttribute("key")]
            public string Key { get; set; }

            [XmlText]
            public string Value { get; set; }
        }
    }
}