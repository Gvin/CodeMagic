using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Liquids;

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

        [XmlElement("evaporation-temperature-multiplier")]
        public double EvaporationTemperatureMultiplier { get; set; }

        [XmlElement("condensation-temperature-multiplier")]
        public double CondensationTemperatureMultiplier { get; set; }

        [XmlElement("freezing-temperature-multiplier")]
        public double FreezingTemperatureMultiplier { get; set; }

        [XmlElement("melting-temperature-multiplier")]
        public double MeltingTemperatureMultiplier { get; set; }

        [XmlIgnore]
        public ISteamConfiguration Steam => SteamData;

        [XmlElement("steam")]
        public XmlSteamConfigurationType SteamData { get; set; }

        [XmlIgnore]
        public ILiquidConfigurationCustomValue[] CustomValues =>
            CustomValuesData.ToArray<ILiquidConfigurationCustomValue>();

        [XmlArray("custom")]
        [XmlArrayItem("value")]
        public XmlLiquidConfigurationCustomValueType[] CustomValuesData { get; set; }
    }

    [Serializable]
    public class XmlSteamConfigurationType : ISteamConfiguration
    {
        [XmlElement("pressure-multiplier")]
        public int PressureMultiplier { get; set; }

        [XmlElement("volume-multiplier")]
        public int VolumeMultiplier { get; set; }

        [XmlElement("thickness-multiplier")]
        public double ThicknessMultiplier { get; set; }

        [XmlElement("max-volume-before-spread")]
        public int MaxVolumeBeforeSpread { get; set; }

        [XmlElement("max-spread-volume")]
        public int MaxSpreadVolume { get; set; }
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