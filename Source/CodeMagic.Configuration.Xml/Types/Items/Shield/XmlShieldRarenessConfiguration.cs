using System;
using System.Xml.Serialization;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield;

namespace CodeMagic.Configuration.Xml.Types.Items.Shield
{
    [Serializable]
    public class XmlShieldRarenessConfiguration : IShieldRarenessConfiguration
    {
        [XmlAttribute("value")]
        public ItemRareness Rareness { get; set; }

        [XmlIgnore]
        public IIntervalConfiguration BlocksDamage => BlocksDamageData;

        [XmlElement("blocks-damage")]
        public XmlIntervalConfiguration BlocksDamageData { get; set; }

        [XmlIgnore]
        public IIntervalConfiguration ProtectChance => ProtectChanceData;

        [XmlElement("protect-chance")]
        public XmlIntervalConfiguration ProtectChanceData { get; set; }

        [XmlIgnore]
        public IIntervalConfiguration HitChancePenalty => HitChancePenaltyData;

        [XmlElement("hit-chance-penalty")]
        public XmlIntervalConfiguration HitChancePenaltyData { get; set; }

        [XmlIgnore]
        public IIntervalConfiguration Bonuses => BonusesData;

        [XmlElement("bonuses")]
        public XmlIntervalConfiguration BonusesData { get; set; }

        [XmlArray("materials")]
        [XmlArrayItem("material")]
        public ItemMaterial[] Materials { get; set; }
    }
}