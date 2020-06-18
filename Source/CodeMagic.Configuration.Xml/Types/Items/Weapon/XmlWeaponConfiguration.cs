using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon
{
    [Serializable]
    public class XmlWeaponConfiguration : IWeaponConfiguration
    {
        [XmlIgnore]
        public ILayersImagesConfiguration Images => ImagesData;

        [XmlElement("images")]
        public XmlLayersImagesConfiguration ImagesData { get; set; }

        [XmlElement("equipped-image-right")]
        public string EquippedImageRight { get; set; }

        [XmlElement("equipped-image-left")]
        public string EquippedImageLeft { get; set; }

        [XmlIgnore]
        public IWeightConfiguration[] Weight => WeightData.ToArray<IWeightConfiguration>();

        [XmlArray("weight")]
        [XmlArrayItem("value")]
        public XmlWeightConfiguration[] WeightData { get; set; }

        [XmlIgnore]
        public IWeaponRarenessConfiguration[] RarenessConfiguration => 
            RarenessConfigurationData.ToArray<IWeaponRarenessConfiguration>();

        [XmlArray("rareness-configuration")]
        [XmlArrayItem("rareness")]
        public XmlWeaponRarenessConfiguration[] RarenessConfigurationData { get; set; }
    }
}