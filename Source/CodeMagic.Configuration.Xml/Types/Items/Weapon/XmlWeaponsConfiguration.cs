using System;
using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Items.Description;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon
{
    [Serializable]
    public class XmlWeaponsConfiguration : IWeaponsConfiguration
    {
        [XmlIgnore]
        public IWeaponConfiguration SwordsConfiguration => SwordsConfigurationData;

        [XmlElement("swords")]
        public XmlWeaponConfiguration SwordsConfigurationData { get; set; }

        [XmlIgnore]
        public IWeaponConfiguration DaggersConfiguration => DaggersConfigurationData;


        [XmlElement("daggers")]
        public XmlWeaponConfiguration DaggersConfigurationData { get; set; }

        [XmlIgnore]
        public IWeaponConfiguration MacesConfiguration => MacesConfigurationData;

        [XmlElement("maces")]
        public XmlWeaponConfiguration MacesConfigurationData { get; set; }

        [XmlIgnore]
        public IWeaponConfiguration AxesConfiguration => AxesConfigurationData;

        [XmlElement("axes")]
        public XmlWeaponConfiguration AxesConfigurationData { get; set; }

        [XmlIgnore]
        public IWeaponConfiguration StaffsConfiguration => StaffsConfigurationData;

        [XmlElement("staffs")]
        public XmlWeaponConfiguration StaffsConfigurationData { get; set; }

        [XmlIgnore]
        public IDescriptionConfiguration DescriptionConfiguration => DescriptionConfigurationData;

        [XmlElement("description")]
        public XmlDescriptionConfiguration DescriptionConfigurationData { get; set; }
    }
}