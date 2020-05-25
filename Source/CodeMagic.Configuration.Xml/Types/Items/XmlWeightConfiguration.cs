using System;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Items
{
    [Serializable]
    public class XmlWeightConfiguration : IWeightConfiguration
    {
        [XmlAttribute("material")]
        public ItemMaterial Material { get; set; }

        [XmlAttribute("weight")]
        public int Weight { get; set; }

        [XmlAttribute("durability")]
        public int Durability { get; set; }
    }
}