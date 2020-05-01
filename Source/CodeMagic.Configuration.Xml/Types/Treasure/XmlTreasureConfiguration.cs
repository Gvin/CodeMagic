using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Treasure;

namespace CodeMagic.Configuration.Xml.Types.Treasure
{
    [XmlRoot("treasure")]
    public class XmlTreasureConfiguration : ITreasureConfiguration
    {
        [XmlIgnore]
        public ITreasureLevelsConfiguration[] Levels => LevelsData.Cast<ITreasureLevelsConfiguration>().ToArray();

        [XmlElement("levels")]
        public XmlTreasureLevelsConfiguration[] LevelsData { get; set; }
    }

    [Serializable]
    public class XmlTreasureLevelsConfiguration : ITreasureLevelsConfiguration
    {
        [XmlAttribute("start-level")]
        public int StartLevel { get; set; }

        [XmlAttribute("end-level")]
        public int EndLevel { get; set; }

        [XmlIgnore]
        public Dictionary<string, ILootConfiguration> Loot =>
            LootData.ToDictionary(data => data.ContainerType, data => (ILootConfiguration) data.Loot);

        [XmlElement("container")]
        public XmlContainerLootConfiguration[] LootData { get; set; }
    }

    [Serializable]
    public class XmlContainerLootConfiguration
    {
        [XmlAttribute("type")]
        public string ContainerType { get; set; }

        [XmlElement("loot")]
        public XmlLootConfiguration Loot { get; set; }
    }
}