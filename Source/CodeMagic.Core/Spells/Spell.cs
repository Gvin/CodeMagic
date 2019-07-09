using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Spells
{
    public class Spell
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int ManaCost { get; set; }

        public CodeSpell CreateCodeSpell(ICreatureObject caster)
        {
            return new CodeSpell(caster, Name, Code, ManaCost);
        }
    }
}