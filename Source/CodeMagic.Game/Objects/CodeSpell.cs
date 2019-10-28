using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Spells;
using CodeMagic.Game.Spells.Script;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects
{
    public class CodeSpell : IMapObject, ILightObject, IDynamicObject, IWorldImageProvider
    {
        private const string ImageHighMana = "Spell_HighMana";
        private const string ImageMediumMana = "Spell_MediumMana";
        private const string ImageLowMana = "Spell_LowMana";

        private const int HighManaLevel = 100;
        private const int MediumManaLevel = 20;

        private readonly AnimationsBatchManager animations;
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

            animations = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.OneByOneStartFromRandom);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public UpdateOrder UpdateOrder => UpdateOrder.Early;

        public bool Updated { get; set; }

        public int Mana { get; set; }

        public void SetEmitLight(LightLevel level, int time)
        {
            LightPower = level;
            remainingLightTime = time;
        }

        private LightLevel LightPower { get; set; }

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            var currentPosition = position;
            try
            {
                ProcessLightEmitting();

                var action = codeExecutor.Execute(map, journal, position, this, lifeTime);
                lifeTime++;

                if (action.ManaCost <= Mana)
                {
                    currentPosition = action.Perform(map, journal, position);
                    Mana -= action.ManaCost;
                }
                else
                {
                    Mana = 0;
                }

                if (Mana != 0)
                    return;

                journal.Write(new SpellOutOfManaMessage(Name));
                map.RemoveObject(currentPosition, this);
            }
            catch (SpellException ex)
            {
                journal.Write(new SpellErrorMessage(Name, ex.Message));
                map.RemoveObject(currentPosition, this);
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

        public bool BlocksAttack => false;

        public ZIndex ZIndex => ZIndex.Spell;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightPower)
        };

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (Mana >= HighManaLevel)
                return animations.GetImage(storage, ImageHighMana);
            if (Mana >= MediumManaLevel)
                return animations.GetImage(storage, ImageMediumMana);
            return animations.GetImage(storage, ImageLowMana);
        }
    }
}