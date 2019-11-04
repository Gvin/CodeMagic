using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Tool;

namespace CodeMagic.Configuration.Xml.Types.Items.Tool
{
    [Serializable]
    public class XmlToolConfiguration : IToolConfiguration
    {
        [XmlElement("inventory-image")]
        public string InventoryImageTemplate { get; set; }

        [XmlIgnore]
        public IWeightConfiguration[] Weight => WeightData.ToArray<IWeightConfiguration>();

        [XmlArray("weight")]
        [XmlArrayItem("value")]
        public XmlWeightConfiguration[] WeightData { get; set; }

        [XmlIgnore]
        public IToolRarenessConfiguration[] RarenessConfiguration => RarenessConfigurationData.ToArray<IToolRarenessConfiguration>();

        [XmlArray("rareness-configuration")]
        [XmlArrayItem("rareness")]
        public XmlToolRarenessConfiguration[] RarenessConfigurationData { get; set; }
    }
}