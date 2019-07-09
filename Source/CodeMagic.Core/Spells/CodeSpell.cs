using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells
{
    public class CodeSpell : IDynamicObject, IMapObject
    {
        private readonly SpellCodeExecutor codeExecutor;

        public CodeSpell(ICreatureObject caster, string name, string code, int mana)
        {
            Name = name;
            Mana = mana;

            codeExecutor = new SpellCodeExecutor(caster, code);
        }

        public bool Updated { get; set; }

        public int Mana { get; set; }

        public void Update(IAreaMap map, Point position, Journal journal)
        {
            var currentPosition = position;
            try
            {
                var action = codeExecutor.Execute(map, position, this);

                if (action.ManaCost <= Mana)
                {
                    currentPosition = action.Perform(map, position);
                    Mana -= action.ManaCost;
                }
                else
                {
                    Mana = 0;
                }

                if (Mana != 0)
                    return;

                journal.Write(new SpellOutOfManaMessage(Name));
                var cell = map.GetCell(currentPosition);
                cell.Objects.Remove(this);
            }
            catch (SpellException ex)
            {
                journal.Write(new SpellErrorMessage(Name, ex.Message));
                var cell = map.GetCell(currentPosition);
                cell.Objects.Remove(this);
            }
        }

        public string Name { get; }
        public bool BlocksMovement => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;
        public bool BlocksProjectiles => false;
    }
}