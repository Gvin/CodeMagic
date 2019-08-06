using System;
using System.Xml.Serialization;
using CodeMagic.ItemsGeneration;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Description;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon.Description
{
    [Serializable]
    public class XmlMaterialDescriptionConfiguration : IMaterialDescriptionConfiguration
    {
        [XmlAttribute("value")]
        public ItemMaterial Material { get; set; }

        [XmlElement("text")]
        public string[] Text { get; set; }
    }
}