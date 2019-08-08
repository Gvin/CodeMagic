using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Configuration.Xml.Types.Items.Bonuses
{
    [Serializable]
    public class XmlItemGroupBonusesConfiguration : IItemGroupBonusesConfiguration
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("inherit-from")]
        public string InheritFrom { get; set; }

        [XmlIgnore]
        public IBonusRarenessConfiguration[] Configuration => ConfigurationData.ToArray<IBonusRarenessConfiguration>();

        [XmlElement("rareness")]
        public XmlBonusRarenessConfiguration[] ConfigurationData { get; set; }
    }
}