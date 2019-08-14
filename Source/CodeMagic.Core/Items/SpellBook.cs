using CodeMagic.Core.Spells;

namespace CodeMagic.Core.Items
{
    public class SpellBook : EquipableItem
    {
        public SpellBook(SpellBookConfiguration configuration) 
            : base(configuration)
        {
            Spells = new BookSpell[configuration.Size];
        }

        public BookSpell[] Spells { get; }

        public int BookSize => Spells.Length;

        public override bool Stackable => false;
    }

    public class SpellBookConfiguration : EquipableItemConfiguration
    {
        public int Size { get; set; }
    }
}