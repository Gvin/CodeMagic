using CodeMagic.Core.Spells;

namespace CodeMagic.Core.Items
{
    public class SpellBook : Item
    {
        public SpellBook(SpellBookConfiguration configuration) 
            : base(configuration)
        {
            Spells = new Spell[configuration.Size];
        }

        public Spell[] Spells { get; }

        public int Size => Spells.Length;
    }

    public class SpellBookConfiguration : ItemConfiguration
    {
        public int Size { get; set; }
    }
}