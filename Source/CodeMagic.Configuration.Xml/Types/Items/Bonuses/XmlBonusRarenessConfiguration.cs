using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Core.Items;
using CodeMagic.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Configuration.Xml.Types.Items.Bonuses
{
    [Serializable]
    public class XmlBonusRarenessConfiguration : IBonusRarenessConfiguration
    {
        [XmlAttribute("value")]
        public ItemRareness Rareness { get; set; }

        [XmlIgnore]
        public IBonusConfiguration[] Bonuses => BonusesData.ToArray<IBonusConfiguration>();

        [XmlElement("bonus")]
        public XmlBonusConfiguration[] BonusesData { get; set; }
    }
}