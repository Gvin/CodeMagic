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

        [XmlText]
        public int Weight { get; set; }
    }
}