using System;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon.Head;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon.Head
{
    [Serializable]
    public class XmlHeadImagesConfiguration : IHeadImagesConfiguration
    {
        [XmlArray("handle")]
        [XmlArrayItem("image")]
        public string[] HandleImages { get; set; }

        [XmlArray("shaft")]
        [XmlArrayItem("image")]
        public string[] ShaftImages { get; set; }

        [XmlArray("head")]
        [XmlArrayItem("image")]
        public string[] HeadImages { get; set; }
    }
}