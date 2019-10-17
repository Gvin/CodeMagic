using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Configuration.Xml.Types.Items.Bonuses
{
    [Serializable]
    public class XmlBonusesConfiguration : IBonusesConfiguration
    {
        [XmlIgnore]
        public IItemGroupBonusesConfiguration[] Groups => GroupsData.ToArray<IItemGroupBonusesConfiguration>();

        [XmlElement("group")]
        public XmlItemGroupBonusesConfiguration[] GroupsData { get; set; }
    }
}