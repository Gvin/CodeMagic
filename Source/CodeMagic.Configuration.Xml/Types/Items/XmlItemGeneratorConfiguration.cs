using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Items.Armor;
using CodeMagic.Configuration.Xml.Types.Items.Bonuses;
using CodeMagic.Configuration.Xml.Types.Items.SpellBook;
using CodeMagic.Configuration.Xml.Types.Items.Tool;
using CodeMagic.Configuration.Xml.Types.Items.Weapon;
using CodeMagic.ItemsGeneration.Configuration;
using CodeMagic.ItemsGeneration.Configuration.Armor;
using CodeMagic.ItemsGeneration.Configuration.Bonuses;
using CodeMagic.ItemsGeneration.Configuration.SpellBook;
using CodeMagic.ItemsGeneration.Configuration.Tool;
using CodeMagic.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.Configuration.Xml.Types.Items
{
    [XmlRoot("item-generator-configuration")]
    public class XmlItemGeneratorConfiguration : IItemGeneratorConfiguration
    {
        [XmlIgnore]
        public IWeaponConfiguration WeaponConfiguration => WeaponConfigurationData;

        [XmlElement("weapon")]
        public XmlWeaponConfiguration WeaponConfigurationData { get; set; }

        [XmlIgnore]
        public IArmorConfiguration ArmorConfiguration => ArmorConfigurationData;

        [XmlElement("armor")]
        public XmlArmorConfiguration ArmorConfigurationData { get; set; }

        [XmlIgnore]
        public ISpellBooksConfiguration SpellBooksConfiguration => SpellBooksConfigurationData;

        [XmlElement("spell-book")]
        public XmlSpellBooksConfiguration SpellBooksConfigurationData { get; set; }

        [XmlIgnore]
        public IBonusesConfiguration BonusesConfiguration => BonusesConfigurationData;

        [XmlElement("bonuses")]
        public XmlBonusesConfiguration BonusesConfigurationData { get; set; }

        [XmlIgnore]
        public IToolsConfiguration ToolsConfiguration => ToolsConfigurationData;

        [XmlElement("tools")]
        public XmlToolsConfiguration ToolsConfigurationData { get; set; }
    }
}