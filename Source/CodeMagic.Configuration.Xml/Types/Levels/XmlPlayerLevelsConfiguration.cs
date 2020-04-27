using System;
using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Levels;

namespace CodeMagic.Configuration.Xml.Types.Levels
{
    [Serializable]
    public class XmlPlayerLevelsConfiguration : IPlayerLevelsConfiguration
    {
        [XmlElement("xp-multiplier")]
        public int XpMultiplier { get; set; }

        [XmlElement("xp-level-power")]
        public int XpLevelPower { get; set; }
    }
}