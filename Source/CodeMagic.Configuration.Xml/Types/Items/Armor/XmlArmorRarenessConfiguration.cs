using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor;

namespace CodeMagic.Configuration.Xml.Types.Items.Armor
{
    [Serializable]
    public class XmlArmorRarenessConfiguration : IArmorRarenessConfiguration
    {
        [XmlAttribute("value")]
        public ItemRareness Rareness { get; set; }

        [XmlIgnore]
        public IElementConfiguration[] Protection => ProtectionData.ToArray<IElementConfiguration>();

        [XmlArray("protection")]
        [XmlArrayItem("value")]
        public XmlElementConfiguration[] ProtectionData { get; set; }

        [XmlElement("min-bonuses")]
        public int MinBonuses { get; set; }

        [XmlElement("max-bonuses")]
        public int MaxBonuses { get; set; }

        [XmlArray("materials")]
        [XmlArrayItem("material")]
        public ItemMaterial[] Materials { get; set; }
    }
}