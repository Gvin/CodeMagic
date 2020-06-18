using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield;

namespace CodeMagic.Configuration.Xml.Types.Items.Shield
{
    [Serializable]
    public class XmlShieldConfiguration : IShieldConfiguration
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("world-image")]
        public string WorldImage { get; set; }

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
        public IShieldRarenessConfiguration[] RarenessConfiguration =>
            RarenessConfigurationData.ToArray<IShieldRarenessConfiguration>();

        [XmlArray("rareness-configuration")]
        [XmlArrayItem("rareness")]
        public XmlShieldRarenessConfiguration[] RarenessConfigurationData { get; set; }
    }
}