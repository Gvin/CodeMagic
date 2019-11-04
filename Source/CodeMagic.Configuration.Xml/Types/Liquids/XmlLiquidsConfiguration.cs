using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Liquids;

namespace CodeMagic.Configuration.Xml.Types.Liquids
{
    [XmlRoot("liquids-configuration")]
    public class XmlLiquidsConfiguration : ILiquidsConfiguration
    {
        [XmlIgnore]
        public ILiquidConfiguration[] LiquidsConfigurations =>
            LiquidsConfigurationsData.ToArray<ILiquidConfiguration>();

        [XmlElement("liquid")]
        public XmlLiquidConfigurationType[] LiquidsConfigurationsData { get; set; }
    }
}