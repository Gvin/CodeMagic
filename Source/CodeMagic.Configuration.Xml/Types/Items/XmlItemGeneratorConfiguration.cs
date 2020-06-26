using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Items.Armor;
using CodeMagic.Configuration.Xml.Types.Items.Bonuses;
using CodeMagic.Configuration.Xml.Types.Items.Shield;
using CodeMagic.Configuration.Xml.Types.Items.SpellBook;
using CodeMagic.Configuration.Xml.Types.Items.Weapon;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.SpellBook;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.Configuration.Xml.Types.Items
{
    [XmlRoot("item-generator-configuration")]
    public class XmlItemGeneratorConfiguration : IItemGeneratorConfiguration
    {
        [XmlIgnore]
        public IWeaponsConfiguration WeaponsConfiguration => WeaponsConfigurationData;

        [XmlElement("weapon")]
        public XmlWeaponsConfiguration WeaponsConfigurationData { get; set; }

        [XmlIgnore]
        public IArmorConfiguration ArmorConfiguration => ArmorConfigurationData;

        [XmlElement("armor")]
        public XmlArmorConfiguration ArmorConfigurationData { get; set; }

        [XmlIgnore]
        public IShieldsConfiguration ShieldsConfiguration => ShieldsConfigurationData;

        [XmlElement("shields")]
        public XmlShieldsConfiguration ShieldsConfigurationData { get; set; }

        [XmlIgnore]
        public ISpellBooksConfiguration SpellBooksConfiguration => SpellBooksConfigurationData;

        [XmlElement("spell-book")]
        public XmlSpellBooksConfiguration SpellBooksConfigurationData { get; set; }

        [XmlIgnore]
        public IBonusesConfiguration BonusesConfiguration => BonusesConfigurationData;

        [XmlElement("bonuses")]
        public XmlBonusesConfiguration BonusesConfigurationData { get; set; }
    }
}