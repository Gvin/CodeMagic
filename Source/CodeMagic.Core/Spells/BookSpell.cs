using CodeMagic.Core.Injection;
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
            return Injector.Current.Create<ICodeSpell>(caster, Name, Code, ManaCost);
        }
    }
}