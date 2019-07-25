using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Spells
{
    public class BookSpell
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int ManaCost { get; set; }

        public ICodeSpell CreateCodeSpell(ICreatureObject caster)
        {
            return MapObjectsFactory.CreateCodeSpell(caster, Name, Code, ManaCost);
        }
    }
}