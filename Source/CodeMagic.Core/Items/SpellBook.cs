using CodeMagic.Core.Spells;

namespace CodeMagic.Core.Items
{
    public class SpellBook : Item, IEquipableItem
    {
        public SpellBook(SpellBookConfiguration configuration) 
            : base(configuration)
        {
            Spells = new BookSpell[configuration.Size];
        }

        public BookSpell[] Spells { get; }

        public int Size => Spells.Length;

        public override bool Stackable => false;
    }

    public class SpellBookConfiguration : ItemConfiguration
    {
        public int Size { get; set; }
    }
}