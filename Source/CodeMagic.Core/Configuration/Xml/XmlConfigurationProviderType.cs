using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CodeMagic.Core.Configuration.Xml
{
    [Serializable]
    [XmlRoot("configuration")]
    public class XmlConfigurationProviderType : IConfigurationProvider
    {
        [XmlIgnore]
        public ISpellConfiguration[] SpellsConfiguration => SpellsConfigurationData.ToArray<ISpellConfiguration>();

        [XmlIgnore] public ITemperatureConfiguration TemperatureConfiguration => TemperatureConfigurationData;

        [XmlElement("temperature")]
        public XmlTemperatureConfigurationType TemperatureConfigurationData { get; set; }

        [XmlIgnore]
        public IPressureConfiguration PressureConfiguration => PressureConfigurationData;

        [XmlElement("pressure")]
        public XmlPressureConfigurationType PressureConfigurationData { get; set; }

        [XmlArray("spells")]
        [XmlArrayItem("spell")]
        public XmlSpellConfigurationType[] SpellsConfigurationData { get; set; }

        public static XmlConfigurationProviderType LoadConfiguration(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                var serializer = new XmlSerializer(typeof(XmlConfigurationProviderType));
                return serializer.Deserialize(file) as XmlConfigurationProviderType;
            }
        }
    }
}