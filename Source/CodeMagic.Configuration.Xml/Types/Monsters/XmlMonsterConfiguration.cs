using System;
using System.Xml.Serialization;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Configuration.Xml.Types.Monsters
{
    [Serializable]
    public class XmlMonsterConfiguration : IMonsterConfiguration
    {
        [XmlIgnore]
        public IMonsterSpawnConfiguration SpawnConfiguration => SpawnConfigurationData;

        [XmlElement("spawn-configuration")]
        public XmlMonsterSpawnConfiguration SpawnConfigurationData { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("logic-pattern")]
        public string LogicPattern { get; set; }

        [XmlIgnore]
        public IMonsterExperienceConfiguration Experience => ExperienceData;

        [XmlElement("experience")]
        public XmlMonsterExperienceConfiguration ExperienceData { get; set; }

        [XmlElement("image")]
        public string Image { get; set; }

        [XmlElement("size")]
        public ObjectSize Size { get; set; }

        [XmlElement("remains-type")]
        public RemainsType RemainsType { get; set; }

        [XmlElement("damage-mark-type")]
        public RemainsType DamageMarkType { get; set; }

        [XmlIgnore]
        public IMonsterStatsConfiguration Stats => StatsData;

        [XmlElement("stats")]
        public XmlMonsterStatsConfiguration StatsData { get; set; }

        [XmlIgnore]
        public ILootConfiguration Loot => LootData;

        [XmlElement("loot")]
        public XmlLootConfiguration LootData { get; set; }
    }

    [Serializable]
    public class XmlMonsterSpawnConfiguration : IMonsterSpawnConfiguration
    {
        [XmlAttribute("min-level")]
        public int MinLevel { get; set; }

        [XmlAttribute("force")]
        public int Force { get; set; }

        [XmlAttribute("group")]
        public string Group { get; set; }
    }

    [Serializable]
    public class XmlMonsterExperienceConfiguration : IMonsterExperienceConfiguration
    {
        [XmlAttribute("max")]
        public int Max { get; set; }

        [XmlAttribute("min")]
        public int Min { get; set; }
    }
}