using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon.Head;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon.Head
{
    [Serializable]
    public class XmlHeadWeaponConfiguration : IHeadWeaponConfiguration
    {
        [XmlIgnore]
        public IHeadImagesConfiguration Images => ImagesData;

        [XmlElement("images")]
        public XmlHeadImagesConfiguration ImagesData { get; set; }

        [XmlIgnore]
        public IWeightConfiguration[] Weight => WeightData.ToArray<IWeightConfiguration>();

        [XmlArray("weight")]
        [XmlArrayItem("value")]
        public XmlWeightConfiguration[] WeightData { get; set; }

        [XmlIgnore]
        public IWeaponRarenessConfiguration[] RarenessConfiguration => RarenessConfigurationData.ToArray<IWeaponRarenessConfiguration>();

        [XmlArray("rareness-configuration")]
        [XmlArrayItem("rareness")]
        public XmlWeaponRarenessConfiguration[] RarenessConfigurationData { get; set; }
    }
}