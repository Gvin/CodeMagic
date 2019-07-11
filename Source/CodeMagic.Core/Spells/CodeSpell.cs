using CodeMagic.Core.Game;
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

        public void Update(IGameCore game, Point position)
        {
            var currentPosition = position;
            try
            {
                var action = codeExecutor.Execute(game, position, this);

                if (action.ManaCost <= Mana)
                {
                    currentPosition = action.Perform(game, position);
                    Mana -= action.ManaCost;
                }
                else
                {
                    Mana = 0;
                }

                if (Mana != 0)
                    return;

                game.Journal.Write(new SpellOutOfManaMessage(Name));
                var cell = game.Map.GetCell(currentPosition);
                cell.Objects.Remove(this);
            }
            catch (SpellException ex)
            {
                game.Journal.Write(new SpellErrorMessage(Name, ex.Message));
                var cell = game.Map.GetCell(currentPosition);
                cell.Objects.Remove(this);
            }
        }

        public string Name { get; }
        public bool BlocksMovement => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;
        public bool BlocksProjectiles => false;
        public bool BlocksEnvironment => false;
    }
}