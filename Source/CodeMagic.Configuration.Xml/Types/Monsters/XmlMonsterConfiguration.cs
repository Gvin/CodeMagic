using System;
using System.Linq;
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
        public IMonsterSpawnConfiguration[] SpawnConfiguration => SpawnConfigurationData.ToArray<IMonsterSpawnConfiguration>();

        [XmlArray("spawn-configuration")]
        [XmlArrayItem("level")]
        public XmlMonsterSpawnConfiguration[] SpawnConfigurationData { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("logic-pattern")]
        public string LogicPattern { get; set; }

        [XmlIgnore]
        public IMonsterImagesConfiguration Images => ImagesData;

        [XmlElement("images")]
        public XmlMonsterImagesConfiguration ImagesData { get; set; }

        [XmlElement("size")]
        public ObjectSize Size { get; set; }

        [XmlElement("remains-type")]
        public RemainsType RemainsType { get; set; }

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
        [XmlAttribute("value")]
        public int Level { get; set; }

        [XmlAttribute("rate")]
        public int Rate { get; set; }
    }
}