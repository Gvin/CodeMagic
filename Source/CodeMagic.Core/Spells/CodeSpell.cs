using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Spells.Script;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Core.Spells
{
    public interface ICodeSpell : IMapObject, ILightObject, IInjectable
    {
        int Mana { get; set; }

        void SetEmitLight(LightLevel level, int time);
    }

    public class CodeSpell : ICodeSpell, IDynamicObject
    {
        public const LightLevel DefaultLightLevel = LightLevel.Dusk1;
        private readonly SpellCodeExecutor codeExecutor;
        private int? remainingLightTime;
        private int lifeTime;

        public CodeSpell(ICreatureObject caster, string name, string code, int mana)
        {
            Name = name;
            Mana = mana;
            LightPower = DefaultLightLevel;
            remainingLightTime = null;
            lifeTime = 0;

            codeExecutor = new SpellCodeExecutor(caster, code);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public UpdateOrder UpdateOrder => UpdateOrder.Early;

        public bool Updated { get; set; }

        public int Mana { get; set; }

        public bool IsLightOn => true;

        public void SetEmitLight(LightLevel level, int time)
        {
            LightPower = level;
            remainingLightTime = time;
        }

        private LightLevel LightPower { get; set; }

        public void Update(IGameCore game, Point position)
        {
            var currentPosition = position;
            try
            {
                ProcessLightEmitting();

                var action = codeExecutor.Execute(game, position, this, lifeTime);
                lifeTime++;

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

        private void ProcessLightEmitting()
        {
            if (!remainingLightTime.HasValue)
                return;

            if (remainingLightTime.Value < 0)
            {
                LightPower = DefaultLightLevel;
                remainingLightTime = null;
                return;
            }

            remainingLightTime = remainingLightTime.Value - 1;
        }

        public string Name { get; }
        public bool BlocksMovement => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;
        public bool BlocksProjectiles => false;
        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.Spell;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightPower, Color.Red)
        };
    }
}