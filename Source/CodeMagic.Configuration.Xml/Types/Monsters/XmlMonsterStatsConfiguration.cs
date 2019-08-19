using System;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration.Monsters;

namespace CodeMagic.Configuration.Xml.Types.Monsters
{
    [Serializable]
    public class XmlMonsterStatsConfiguration : IMonsterStatsConfiguration
    {
        [XmlIgnore]
        public int MaxHealth => HealthConfiguration.Max;

        [XmlIgnore]
        public int MinHealth => HealthConfiguration.Min;

        [XmlElement("health")]
        public XmlMonsterHealthConfiguration HealthConfiguration { get; set; }

        [XmlElement("speed")]
        public float Speed { get; set; }
        
        [XmlElement("catch-fire-chance-multiplier")]
        public int CatchFireChanceMultiplier { get; set; }

        [XmlElement("self-extinguish-chance")]
        public int SelfExtinguishChanceMultiplier { get; set; }

        [XmlElement("visibility-range")]
        public int VisibilityRange { get; set; }

        [XmlElement("hit-chance")]
        public int HitChance { get; set; }

        [XmlIgnore]
        public IMonsterProtectionConfiguration[] Protection => ProtectionData;

        [XmlArray("protection")]
        [XmlArrayItem("element")]
        public XmlMonsterProtectionConfiguration[] ProtectionData { get; set; }

        [XmlIgnore]
        public IMonsterDamageConfiguration[] Damage => DamageData;

        [XmlArray("damage")]
        [XmlArrayItem("value")]
        public XmlMonsterDamageConfiguration[] DamageData { get; set; }

        [Serializable]
        public class XmlMonsterHealthConfiguration
        {
            [XmlAttribute("min")]
            public int Min { get; set; }

            [XmlAttribute("max")]
            public int Max { get; set; }
        }
    }
}