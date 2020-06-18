using System;
using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Items.Description;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield;

namespace CodeMagic.Configuration.Xml.Types.Items.Shield
{
    [Serializable]
    public class XmlShieldsConfiguration : IShieldsConfiguration
    {
        [XmlIgnore]
        public IShieldConfiguration SmallShieldConfiguration => SmallShieldConfigurationData;

        [XmlElement("small")]
        public XmlShieldConfiguration SmallShieldConfigurationData { get; set; }

        [XmlIgnore]
        public IShieldConfiguration MediumShieldConfiguration => MediumShieldConfigurationData;

        [XmlElement("medium")]
        public XmlShieldConfiguration MediumShieldConfigurationData { get; set; }

        [XmlIgnore]
        public IShieldConfiguration BigShieldConfiguration => BigShieldConfigurationData;

        [XmlElement("big")]
        public XmlShieldConfiguration BigShieldConfigurationData { get; set; }

        [XmlIgnore]
        public IDescriptionConfiguration DescriptionConfiguration => DescriptionConfigurationData;

        [XmlElement("description")]
        public XmlDescriptionConfiguration DescriptionConfigurationData { get; set; }
    }
}