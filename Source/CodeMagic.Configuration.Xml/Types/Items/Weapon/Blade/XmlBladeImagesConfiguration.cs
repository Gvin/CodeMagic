using System;
using System.Xml.Serialization;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Blade;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon.Blade
{
    [Serializable]
    public class XmlBladeImagesConfiguration : IBladeImagesConfiguration
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