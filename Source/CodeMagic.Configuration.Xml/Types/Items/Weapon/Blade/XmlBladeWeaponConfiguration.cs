using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon.Blade;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon.Blade
{
    [Serializable]
    public class XmlBladeWeaponConfiguration : IBladeWeaponConfiguration
    {
        [XmlIgnore]
        public IBladeImagesConfiguration Images => ImagesData;

        [XmlElement("images")]
        public XmlBladeImagesConfiguration ImagesData { get; set; }

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