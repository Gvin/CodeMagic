using CodeMagic.Core.Spells;

namespace CodeMagic.Core.Items
{
    public class SpellBook : Item
    {
        public SpellBook(SpellBookConfiguration configuration) 
            : base(configuration)
        {
            Spells = new BookSpell[configuration.Size];
        }

        public BookSpell[] Spells { get; }

        public int Size => Spells.Length;
    }

    public class SpellBookConfiguration : ItemConfiguration
    {
        public int Size { get; set; }
    }
}