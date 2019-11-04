using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Spells;

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
                    Code = Properties.Resources.AncientSpell_ThorsHands
                },
                new BookSpell
                {
                    Name = "Lighter",
                    ManaCost = 1000,
                    Code = Properties.Resources.AncientSpell_Lighter
                }
            };

            rareSpells = new[]
            {
                new BookSpell
                {
                    Name = "Shield",
                    ManaCost = 1000,
                    Code = Properties.Resources.AncientSpell_Shield
                },
                new BookSpell
                {
                    Name = "Fireball",
                    ManaCost = 500,
                    Code = Properties.Resources.AncientSpell_Fireball
                }
            };
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