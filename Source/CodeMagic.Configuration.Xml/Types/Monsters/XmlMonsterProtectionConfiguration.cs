using System;
using System.Xml.Serialization;
using CodeMagic.Core.Game;
using CodeMagic.Game.Configuration.Monsters;

namespace CodeMagic.Configuration.Xml.Types.Monsters
{
    [Serializable]
    public class XmlMonsterProtectionConfiguration : IMonsterProtectionConfiguration
    {
        [XmlAttribute("type")]
        public Element Element { get; set; }

        [XmlText]
        public int Value { get; set; }
    }
}