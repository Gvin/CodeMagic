using System.IO;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Spells;
using Path = System.IO.Path;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class AncientSpellsProvider : IAncientSpellsProvider
    {
        private readonly BookSpell[] uncommonSpells;
        private readonly BookSpell[] rareSpells;

        public AncientSpellsProvider()
        {
            uncommonSpells = new[]
            {
                new BookSpell
                {
                    Name = "Thor's Hands",
                    ManaCost = 300,
                    Code = LoadSpellCode("AncientSpell_ThorsHands")
                },
                new BookSpell
                {
                    Name = "Lighter",
                    ManaCost = 1000,
                    Code = LoadSpellCode("AncientSpell_Lighter")
                }
            };

            rareSpells = new[]
            {
                new BookSpell
                {
                    Name = "Shield",
                    ManaCost = 1000,
                    Code = LoadSpellCode("AncientSpell_Shield")
                },
                new BookSpell
                {
                    Name = "Fireball",
                    ManaCost = 500,
                    Code = LoadSpellCode("AncientSpell_Fireball")
                }
            };
        }

        private string LoadSpellCode(string spellName)
        {
            var path = Path.Combine(@"./Resources/AncientSpells/", $"{spellName}.js");
            return File.ReadAllText(path);
        }

        public BookSpell[] GetUncommonSpells()
        {
            return uncommonSpells;
        }

        public BookSpell[] GetRareSpells()
        {
            return rareSpells;
        }
    }
}