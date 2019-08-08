using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.ItemsGeneration.Configuration.SpellBook;

namespace CodeMagic.Configuration.Xml.Types.Items.SpellBook
{
    [Serializable]
    public class XmlSpellBooksConfiguration : ISpellBooksConfiguration
    {
        [XmlElement("template")]
        public string Template { get; set; }

        [XmlArray("symbols")]
        [XmlArrayItem("image")]
        public string[] SymbolImages { get; set; }

        [XmlIgnore]
        public ISpellBookRarenessConfiguration[] Configuration => ConfigurationData.ToArray<ISpellBookRarenessConfiguration>();

        [XmlArray("rareness-configuration")]
        [XmlArrayItem("rareness")]
        public XmlSpellBookRarenessConfiguration[] ConfigurationData { get; set; }

        [XmlElement("weight")]
        public int Weight { get; set; }
    }
}