using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Monsters;

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

        [XmlElement("accuracy")]
        public int Accuracy { get; set; }

        [XmlElement("dodge-chance")]
        public int DodgeChance { get; set; }

        [XmlIgnore]
        public IMonsterProtectionConfiguration[] Protection => ProtectionData?.ToArray<IMonsterProtectionConfiguration>() ?? new IMonsterProtectionConfiguration[0];

        [XmlArray("protection")]
        [XmlArrayItem("element")]
        public XmlMonsterProtectionConfiguration[] ProtectionData { get; set; }

        [XmlArray("statuses-immunity")]
        [XmlArrayItem("status")]
        public string[] StatusesImmunity { get; set; }

        [XmlElement("shield-block-chance")]
        public int ShieldBlockChance { get; set; }

        [XmlElement("shield-blocks-damage")]
        public int ShieldBlocksDamage { get; set; }

        [XmlIgnore]
        public IMonsterDamageConfiguration[] Damage => DamageData?.ToArray<IMonsterDamageConfiguration>() ?? new IMonsterDamageConfiguration[0];

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