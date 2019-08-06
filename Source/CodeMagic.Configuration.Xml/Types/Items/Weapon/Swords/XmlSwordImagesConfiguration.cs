using System;
using System.Xml.Serialization;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Swords;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon.Swords
{
    [Serializable]
    public class XmlSwordImagesConfiguration : ISwordImagesConfiguration
    {
        [XmlArray("handle")]
        [XmlArrayItem("image")]
        public string[] HandleImages { get; set; }

        [XmlArray("guard")]
        [XmlArrayItem("image")]
        public string[] GuardImages { get; set; }

        [XmlArray("blade")]
        [XmlArrayItem("image")]
        public string[] BladeImages { get; set; }
    }
}