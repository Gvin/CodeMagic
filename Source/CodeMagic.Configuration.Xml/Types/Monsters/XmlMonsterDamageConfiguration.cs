using System;
using System.Xml.Serialization;
using CodeMagic.Core.Game;
using CodeMagic.Game.Configuration.Monsters;

namespace CodeMagic.Configuration.Xml.Types.Monsters
{
    [Serializable]
    public class XmlMonsterDamageConfiguration : IMonsterDamageConfiguration
    {
        [XmlAttribute("element")]
        public Element Element { get; set; }

        [XmlAttribute("min")]
        public int MinValue { get; set; }

        [XmlAttribute("max")]
        public int MaxValue { get; set; }
    }
}