using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Configuration.Monsters;

namespace CodeMagic.Configuration.Xml.Types.Monsters
{
    [XmlRoot("monsters-configuration")]
    public class XmlMonstersConfiguration : IMonstersConfiguration
    {
        [XmlIgnore]
        public IMonsterConfiguration[] Monsters => MonstersData.ToArray<IMonsterConfiguration>();

        [XmlElement("monster")]
        public XmlMonsterConfiguration[] MonstersData { get; set; }
    }
}