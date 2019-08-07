using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.ItemsGeneration.Configuration.Armor;

namespace CodeMagic.Configuration.Xml.Types.Items.Armor
{
    [Serializable]
    public class XmlArmorConfiguration : IArmorConfiguration
    {
        [XmlIgnore]
        public IArmorPieceConfiguration[] ChestConfiguration =>
            ChestConfigurationData.ToArray<IArmorPieceConfiguration>();

        [XmlArray("chests")]
        [XmlArrayItem("item")]
        public XmlArmorPieceConfiguration[] ChestConfigurationData { get; set; }

        [XmlIgnore]
        public IArmorPieceConfiguration[] LeggingsConfiguration =>
            LeggingsConfigurationData.ToArray<IArmorPieceConfiguration>();

        [XmlArray("leggings")]
        [XmlArrayItem("item", Type = typeof(XmlArmorPieceConfiguration))]
        public XmlArmorPieceConfiguration[] LeggingsConfigurationData { get; set; }

        [XmlIgnore]
        public IArmorPieceConfiguration[] HelmetConfiguration =>
            HelmetConfigurationData.ToArray<IArmorPieceConfiguration>();

        [XmlArray("helmets")]
        [XmlArrayItem("item", Type = typeof(XmlArmorPieceConfiguration))]
        public XmlArmorPieceConfiguration[] HelmetConfigurationData { get; set; }
    }
}