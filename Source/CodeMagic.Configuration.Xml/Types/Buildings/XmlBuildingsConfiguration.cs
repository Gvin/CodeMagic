using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration.Buildings;
using CodeMagic.Core.Items;
using CodeMagic.Implementations.Objects.Buildings;

namespace CodeMagic.Configuration.Xml.Types.Buildings
{
    [XmlRoot("buildings-configuration")]
    public class XmlBuildingsConfiguration : IBuildingsConfiguration
    {
        [XmlIgnore]
        public IBuildingConfiguration[] Buildings => BuildingsData;

        [XmlElement("building")]
        public XmlBuildingConfiguration[] BuildingsData { get; set; }
    }

    [Serializable]
    public class XmlBuildingConfiguration : IBuildingConfiguration
    {
        private static readonly Dictionary<string, Type> Buildings = new Dictionary<string, Type>
        {
            {"palisade", typeof(Palisade)},
            {"palisade_gates", typeof(PalisadeGates)},
            {"palisade_embrasure", typeof(PalisadeEmbrasure)},
            {"box", typeof(Box)},
            {"chest", typeof(Chest)},
            {"furnace", typeof(Furnace)}
        };

        [XmlIgnore]
        public Type Type => Buildings[Id];

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlArray("unlocks")]
        [XmlArrayItem("id")]
        public string[] Unlocks { get; set; }

        [XmlAttribute("auto-unlock")]
        [DefaultValue(false)]
        public bool AutoUnlock { get; set; }

        [XmlArray("description")]
        [XmlArrayItem("l")]
        public string[] Description { get; set; }

        [XmlElement("rareness")]
        public ItemRareness Rareness { get; set; }

        [XmlElement("preview-image")]
        public string PreviewImage { get; set; }

        [XmlElement("build-time")]
        public int BuildTime { get; set; }

        [XmlIgnore]
        public IBuildingMaterialConfiguration[] Cost => CostData;

        [XmlArray("cost")]
        [XmlArrayItem("material")]
        public XmlBuildingMaterial[] CostData { get; set; }
    }

    [Serializable]
    public class XmlBuildingMaterial : IBuildingMaterialConfiguration
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public int Count { get; set; }
    }
}