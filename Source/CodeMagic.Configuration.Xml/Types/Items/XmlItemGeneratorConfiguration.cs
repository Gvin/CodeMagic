using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Items.Armor;
using CodeMagic.Configuration.Xml.Types.Items.Weapon;
using CodeMagic.ItemsGeneration.Configuration;
using CodeMagic.ItemsGeneration.Configuration.Armor;
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
    }
}