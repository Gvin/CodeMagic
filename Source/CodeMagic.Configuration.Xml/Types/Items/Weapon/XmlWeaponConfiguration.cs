using System;
using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Items.Weapon.Swords;
using CodeMagic.ItemsGeneration.Configuration.Weapon;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Swords;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon
{
    [Serializable]
    public class XmlWeaponConfiguration : IWeaponConfiguration
    {
        [XmlIgnore]
        public IBladeWeaponConfiguration SwordsConfiguration => SwordsConfigurationData;

        [XmlElement("swords")]
        public XmlBladeWeaponConfiguration SwordsConfigurationData { get; set; }

        [XmlIgnore]
        public IBladeWeaponConfiguration DaggersConfiguration => DaggersConfigurationData;

        [XmlElement("daggers")]
        public XmlBladeWeaponConfiguration DaggersConfigurationData { get; set; }
    }
}