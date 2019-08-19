using System.Xml.Serialization;
using CodeMagic.Core.Configuration.Monsters;

namespace CodeMagic.Configuration.Xml.Types.Monsters
{
    [XmlRoot("monsters-configuration")]
    public class XmlMonstersConfiguration : IMonstersConfiguration
    {
        [XmlIgnore]
        public IMonsterConfiguration[] Monsters => MonstersData;

        [XmlElement("monster")]
        public XmlMonsterConfiguration[] MonstersData { get; set; }
    }
}