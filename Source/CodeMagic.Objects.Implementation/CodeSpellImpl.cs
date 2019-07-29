using System;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Spells;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation
{
    public class CodeSpellImpl : CodeSpell, IImageProvider
    {
        private const string ImageHighMana = "Spell_HighMana";
        private const string ImageMediumMana = "Spell_MediumMana";
        private const string ImageLowMana = "Spell_LowMana";

        private const int HighManaLevel = 100;
        private const int MediumManaLevel = 20;

        private readonly AnimationsBatchManager animations;

        public CodeSpellImpl(ICreatureObject caster, string name, string code, int mana) 
            : base(caster, name, code, mana)
        {
            animations = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.OneByOneStartFromRandom);
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            if (Mana >= HighManaLevel)
                return animations.GetImage(storage, ImageHighMana);
            if (Mana >= MediumManaLevel)
                return animations.GetImage(storage, ImageMediumMana);
            return animations.GetImage(storage, ImageLowMana);
        }
    }
}