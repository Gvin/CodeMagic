using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CodeMagic.Game.Spells;

namespace CodeMagic.UI.Sad.SpellsLibrary
{
    public class SpellsLibraryManager
    {
        private const string FilePath = ".\\Profile\\SpellsLibrary.xml";

        public void SaveSpell(BookSpell spell)
        {
            var serializer = new XmlSerializer(typeof(XmlSpellsLibrary));
            var library = LoadSpellsLibrary(serializer);
            if (library == null)
            {
                library = new XmlSpellsLibrary();
                var path = Path.GetDirectoryName(FilePath);
                if (!string.IsNullOrEmpty(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            var resultSpells = new List<XmlBookSpell>(library.Spells ?? new XmlBookSpell[0])
            {
                new XmlBookSpell(spell)
            };
            library.Spells = resultSpells.ToArray();

            WriteToXml(library, serializer);
        }

        public void RemoveSpell(BookSpell spell)
        {
            var serializer = new XmlSerializer(typeof(XmlSpellsLibrary));
            var library = LoadSpellsLibrary(serializer);

            var spellToRemove = library?.Spells.FirstOrDefault(xmlSpell => xmlSpell.Equals(spell));
            if (spellToRemove == null)
                return;

            library.Spells = library.Spells.Where(xmlSpell => !ReferenceEquals(xmlSpell, spellToRemove)).ToArray();
            WriteToXml(library, serializer);
        }

        public BookSpell[] ReadSpells()
        {
            var serializer = new XmlSerializer(typeof(XmlSpellsLibrary));
            var library = LoadSpellsLibrary(serializer);
            return library?.Spells?.Select(xmlSpell => xmlSpell.ToBookSpell()).ToArray() ?? new BookSpell[0];
        }

        private XmlSpellsLibrary LoadSpellsLibrary(XmlSerializer serializer)
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }
            using (var file = File.OpenRead(FilePath))
            {
                return serializer.Deserialize(file) as XmlSpellsLibrary;
            }
        }

        private void WriteToXml(XmlSpellsLibrary library, XmlSerializer serializer)
        {
            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                NewLineHandling = NewLineHandling.Entitize
            };
            using (var writer = XmlWriter.Create(FilePath, writerSettings))
            {
                serializer.Serialize(writer, library);
            }
        }
    }

    [XmlRoot("spells-library")]
    public class XmlSpellsLibrary
    {
        [XmlElement("spell")]
        public XmlBookSpell[] Spells { get; set; }
    }

    [Serializable]
    public class XmlBookSpell
    {
        public XmlBookSpell()
        {
        }

        public XmlBookSpell(BookSpell spell)
        {
            Name = spell.Name;
            Code = spell.Code;
            ManaCost = spell.ManaCost;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string Code { get; set; }

        [XmlAttribute("cost")]
        public int ManaCost { get; set; }

        public bool Equals(BookSpell spell)
        {
            return string.Equals(Name, spell.Name) && string.Equals(Code, spell.Code);
        }

        public BookSpell ToBookSpell()
        {
            return new BookSpell
            {
                Name = Name,
                Code = Code,
                ManaCost = ManaCost
            };
        }
    }
}