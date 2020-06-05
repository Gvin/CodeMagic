using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Spells;
using CodeMagic.Game.Spells.Script;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects
{
    public class CodeSpell : MapObjectBase, ILightObject, IDynamicObject, IWorldImageProvider
    {
        private static readonly ILog Log = LogManager.GetLog<CodeSpell>();

        private const string SaveKeyMana = "Mana";
        private const string SaveKeyLightPower = "LightPower";
        private const string SaveKeyLifeTime = "LifeTime";
        private const string SaveKeyRemainingLightTime = "RemainingLightTime";
        private const string SaveKeyCode = "Code";
        private const string SaveKeyCasterId = "CasterId";

        private const string ImageHighMana = "Spell_HighMana";
        private const string ImageMediumMana = "Spell_MediumMana";
        private const string ImageLowMana = "Spell_LowMana";

        private const int HighManaLevel = 100;
        private const int MediumManaLevel = 20;

        private readonly AnimationsBatchManager animations;
        public const LightLevel DefaultLightLevel = LightLevel.Dusk1;
        private readonly SpellCodeExecutor codeExecutor;
        private readonly string casterId;
        private readonly string code;
        private int? remainingLightTime;
        private int lifeTime;

        public CodeSpell(SaveData data)
            : base(data)
        {
            Mana = data.GetIntValue(SaveKeyMana);
            LightPower = (LightLevel) data.GetIntValue(SaveKeyLightPower);
            lifeTime = data.GetIntValue(SaveKeyLifeTime);

            var remainingLightTimeValue = data.GetStringValue(SaveKeyRemainingLightTime);
            remainingLightTime = remainingLightTimeValue == null ? (int?) null : int.Parse(remainingLightTimeValue);

            code = data.GetStringValue(SaveKeyCode);
            casterId = data.GetStringValue(SaveKeyCasterId);
            codeExecutor = new SpellCodeExecutor(casterId, this.code);

            animations = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.OneByOneStartFromRandom);
        }

        public CodeSpell(ICreatureObject caster, string name, string code, int mana)
            : base(name)
        {
            Mana = mana;
            LightPower = DefaultLightLevel;
            remainingLightTime = null;
            lifeTime = 0;

            casterId = caster.Id;
            this.code = code;
            codeExecutor = new SpellCodeExecutor(caster.Id, code);

            animations = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.OneByOneStartFromRandom);
        }

        public override ObjectSize Size => ObjectSize.Huge;

        public UpdateOrder UpdateOrder => UpdateOrder.Early;

        public bool Updated { get; set; }

        public int Mana { get; set; }

        public void SetEmitLight(LightLevel level, int time)
        {
            LightPower = level;
            remainingLightTime = time;
        }

        private LightLevel LightPower { get; set; }

        public void Update(Point position)
        {
            var currentPosition = position;
            try
            {
                ProcessLightEmitting();

                var action = codeExecutor.Execute(position, this, lifeTime);
                lifeTime++;

                if (action.ManaCost <= Mana)
                {
                    currentPosition = action.Perform(position);
                    Mana -= action.ManaCost;
                }
                else
                {
                    Mana = 0;
                }

                if (Mana != 0)
                    return;

                CurrentGame.Journal.Write(new SpellOutOfManaMessage(Name));
                CurrentGame.Map.RemoveObject(currentPosition, this);
            }
            catch (SpellException ex)
            {
                Log.Debug("Spell error", ex);
                CurrentGame.Journal.Write(new SpellErrorMessage(Name, ex.Message));
                CurrentGame.Map.RemoveObject(currentPosition, this);
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

        public override ZIndex ZIndex => ZIndex.Spell;

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

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyMana, Mana);
            data.Add(SaveKeyLightPower, (int) LightPower);
            data.Add(SaveKeyRemainingLightTime, remainingLightTime);
            data.Add(SaveKeyCode, code);
            data.Add(SaveKeyCasterId, casterId);
            data.Add(SaveKeyLifeTime, lifeTime);
            return data;
        }
    }
}