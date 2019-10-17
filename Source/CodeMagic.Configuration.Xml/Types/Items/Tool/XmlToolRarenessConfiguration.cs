using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Tool;

namespace CodeMagic.Configuration.Xml.Types.Items.Tool
{
    [Serializable]
    public class XmlToolRarenessConfiguration : IToolRarenessConfiguration
    {
        [XmlAttribute("value")]
        public ItemRareness Rareness { get; set; }

        [XmlIgnore]
        public IElementConfiguration[] Damage => DamageData.ToArray<IElementConfiguration>();

        [XmlArray("damage")]
        [XmlArrayItem("value")]
        public XmlElementConfiguration[] DamageData { get; set; }

        [XmlElement("min-max-damage-difference")]
        public int MinMaxDamageDifference { get; set; }

        [XmlElement("min-hit-chance")]
        public int MinHitChance { get; set; }

        [XmlElement("max-hit-chance")]
        public int MaxHitChance { get; set; }

        [XmlArray("materials")]
        [XmlArrayItem("material")]
        public ItemMaterial[] Materials { get; set; }

        [XmlElement("max-tool-power")]
        public int MaxToolPower { get; set; }

        [XmlElement("min-tool-power")]
        public int MinToolPower { get; set; }
    }
}