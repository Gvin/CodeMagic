using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Spells;

namespace CodeMagic.Configuration.Xml.Types.Spells
{
    [XmlRoot("spells-configuration")]
    public class XmlSpellsConfigurationType : ISpellsConfiguration
    {
        [XmlIgnore]
        public ISpellConfiguration[] SpellsConfiguration => SpellsConfigurationData.ToArray<ISpellConfiguration>();

        [XmlElement("spell")]
        public XmlSpellConfigurationType[] SpellsConfigurationData { get; set; }
    }
}