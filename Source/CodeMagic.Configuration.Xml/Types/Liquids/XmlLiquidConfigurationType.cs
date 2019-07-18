using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration.Liquids;

namespace CodeMagic.Configuration.Xml.Types.Liquids
{
    [Serializable]
    public class XmlLiquidConfigurationType : ILiquidConfiguration
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("freezing-point")]
        public int FreezingPoint { get; set; }

        [XmlElement("boiling-point")]
        public int BoilingPoint { get; set; }

        [XmlElement("min-volume-for-effect")]
        public int MinVolumeForEffect { get; set; }

        [XmlElement("max-volume-before-spread")]
        public int MaxVolumeBeforeSpread { get; set; }

        [XmlElement("max-spread-volume")]
        public int MaxSpreadVolume { get; set; }

        [XmlElement("evaporation-multiplier")]
        public int EvaporationMultiplier { get; set; }

        [XmlElement("steam-pressure-multiplier")]
        public int SteamPressureMultiplier { get; set; }

        [XmlIgnore]
        public ILiquidConfigurationCustomValue[] CustomValues =>
            CustomValuesData.ToArray<ILiquidConfigurationCustomValue>();

        [XmlArray("custom")]
        [XmlArrayItem("value")]
        public XmlLiquidConfigurationCustomValueType[] CustomValuesData { get; set; }
    }

    [Serializable]
    public class XmlLiquidConfigurationCustomValueType : ILiquidConfigurationCustomValue
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}