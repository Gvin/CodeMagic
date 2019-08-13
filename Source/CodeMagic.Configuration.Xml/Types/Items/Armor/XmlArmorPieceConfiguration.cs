using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Core.Items;
using CodeMagic.ItemsGeneration.Configuration;
using CodeMagic.ItemsGeneration.Configuration.Armor;

namespace CodeMagic.Configuration.Xml.Types.Items.Armor
{
    [Serializable]
    public class XmlArmorPieceConfiguration : IArmorPieceConfiguration
    {
        [XmlAttribute("name")]
        public string TypeName { get; set; }

        [XmlAttribute("class")]
        public ArmorClass Class { get; set; }

        [XmlArray("images")]
        [XmlArrayItem("image")]
        public string[] Images { get; set; }

        [XmlIgnore]
        public IArmorRarenessConfiguration[] RarenessConfigurations => RarenessConfigurationsData.ToArray<IArmorRarenessConfiguration>();

        [XmlArray("rareness-configuration")]
        [XmlArrayItem("rareness")]
        public XmlArmorRarenessConfiguration[] RarenessConfigurationsData { get; set; }

        [XmlIgnore]
        public IWeightConfiguration[] Weight => WeightData.ToArray<IWeightConfiguration>();

        [XmlArray("weight")]
        [XmlArrayItem("value", Type = typeof(XmlWeightConfiguration))]
        public XmlWeightConfiguration[] WeightData { get; set; }
    }
}