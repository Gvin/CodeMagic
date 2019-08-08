using System;
using System.Xml.Serialization;
using CodeMagic.Core.Items;
using CodeMagic.ItemsGeneration.Configuration.SpellBook;

namespace CodeMagic.Configuration.Xml.Types.Items.SpellBook
{
    [Serializable]
    public class XmlSpellBookRarenessConfiguration : ISpellBookRarenessConfiguration
    {
        [XmlAttribute("value")]
        public ItemRareness Rareness { get; set; }

        [XmlElement("min-bonuses")]
        public int MinBonuses { get; set; }

        [XmlElement("max-bonuses")]
        public int MaxBonuses { get; set; }

        [XmlElement("min-spells")]
        public int MinSpells { get; set; }

        [XmlElement("max-spells")]
        public int MaxSpells { get; set; }

        [XmlArray("description")]
        [XmlArrayItem("text")]
        public string[] Description { get; set; }
    }
}