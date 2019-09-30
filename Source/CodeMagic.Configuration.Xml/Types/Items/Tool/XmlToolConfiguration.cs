using System;
using System.Xml.Serialization;
using CodeMagic.ItemsGeneration.Configuration;
using CodeMagic.ItemsGeneration.Configuration.Tool;

namespace CodeMagic.Configuration.Xml.Types.Items.Tool
{
    [Serializable]
    public class XmlToolConfiguration : IToolConfiguration
    {
        [XmlElement("inventory-image")]
        public string InventoryImageTemplate { get; set; }

        [XmlIgnore]
        public IWeightConfiguration[] Weight => WeightData;

        [XmlArray("weight")]
        [XmlArrayItem("value")]
        public XmlWeightConfiguration[] WeightData { get; set; }

        [XmlIgnore]
        public IToolRarenessConfiguration[] RarenessConfiguration => RarenessConfigurationData;

        [XmlArray("rareness-configuration")]
        [XmlArrayItem("rareness")]
        public XmlToolRarenessConfiguration[] RarenessConfigurationData { get; set; }
    }
}