using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal class ManaPotionData : PotionData
    {
        public ManaPotionData(PotionSize size) 
            : base(size, PotionType.Mana)
        {
        }

        public override void Use(GameCore<Player> game)
        {
            var manaValue = GetManaPotionEffect(Size);
            game.Player.Mana += manaValue;
            game.Journal.Write(new ManaRestoredMessage(game.Player, manaValue));
        }

        protected override StyledLine[] GetEffectDescription()
        {
            return new[]
            {
                new StyledLine
                {
                    "Restores ",
                    new StyledString(GetManaPotionEffect(Size).ToString(), TextHelper.ManaColor),
                    " mana when used."
                }
            };
        }

        protected override string GetKnownName(string sizeName)
        {
            return $"{sizeName}Mana Potion";
        }

        private static int GetManaPotionEffect(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return 150;
                case PotionSize.Medium:
                    return 300;
                case PotionSize.Big:
                    return 500;
                default:
                    throw new ArgumentException($"Unknown potion size: {size}", nameof(size));
            }
        }
    }
}