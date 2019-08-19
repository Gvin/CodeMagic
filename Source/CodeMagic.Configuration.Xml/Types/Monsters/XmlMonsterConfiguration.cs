using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Monsters;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.Configuration.Xml.Types.Monsters
{
    [Serializable]
    public class XmlMonsterConfiguration : IMonsterConfiguration
    {
        [XmlIgnore]
        public int[] Levels => LevelsData.Replace(" ", "").Split(',').Select(int.Parse).ToArray();

        [XmlAttribute("levels")]
        public string LevelsData { get; set; }

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
}