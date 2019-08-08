using System;
using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Items.Description;
using CodeMagic.Configuration.Xml.Types.Items.Weapon.Blade;
using CodeMagic.Configuration.Xml.Types.Items.Weapon.Head;
using CodeMagic.ItemsGeneration.Configuration.Description;
using CodeMagic.ItemsGeneration.Configuration.Weapon;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Blade;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Head;

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
        public IDescriptionConfiguration DescriptionConfiguration => DescriptionConfigurationData;

        [XmlElement("description")]
        public XmlDescriptionConfiguration DescriptionConfigurationData { get; set; }
    }
}