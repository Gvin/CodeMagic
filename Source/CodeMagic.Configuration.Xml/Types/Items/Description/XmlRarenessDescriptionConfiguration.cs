using System;
using System.Xml.Serialization;
using CodeMagic.Core.Items;
using CodeMagic.ItemsGeneration.Configuration.Description;

namespace CodeMagic.Configuration.Xml.Types.Items.Description
{
    [Serializable]
    public class XmlRarenessDescriptionConfiguration : IRarenessDescriptionConfiguration
    {
        [XmlAttribute("value")]
        public ItemRareness Rareness { get; set; }

        [XmlElement("text")]
        public string[] Text { get; set; }
    }
}