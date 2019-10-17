using System;
using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Items.Description;
using CodeMagic.Configuration.Xml.Types.Items.Weapon.Blade;
using CodeMagic.Configuration.Xml.Types.Items.Weapon.Head;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon.Blade;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon.Head;

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

        [XmlIgnore]
        public IHeadWeaponConfiguration MacesConfiguration => MacesConfigurationData;

        [XmlElement("maces")]
        public XmlHeadWeaponConfiguration MacesConfigurationData { get; set; }

        [XmlIgnore]
        public IHeadWeaponConfiguration AxesConfiguration => AxesConfigurationData;

        [XmlElement("axes")]
        public XmlHeadWeaponConfiguration AxesConfigurationData { get; set; }

        [XmlIgnore]
        public IHeadWeaponConfiguration StaffsConfiguration => StaffsConfigurationData;

        [XmlElement("staffs")]
        public XmlHeadWeaponConfiguration StaffsConfigurationData { get; set; }

        [XmlIgnore]
        public IDescriptionConfiguration DescriptionConfiguration => DescriptionConfigurationData;

        [XmlElement("description")]
        public XmlDescriptionConfiguration DescriptionConfigurationData { get; set; }
    }
}