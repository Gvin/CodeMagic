using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Levels;

namespace CodeMagic.Configuration.Xml.Types.Levels
{
    [XmlRoot("levels")]
    public class XmlLevelsConfiguration : ILevelsConfiguration
    {
        [XmlIgnore]
        public IPlayerLevelsConfiguration PlayerLevels => PlayerLevelsData;

        [XmlElement("player-levels")]
        public XmlPlayerLevelsConfiguration PlayerLevelsData { get; set; }
    }
}