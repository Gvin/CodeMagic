using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal class StaminaPotionData : PotionData
    {
        public StaminaPotionData(PotionSize size) 
            : base(size, PotionType.Stamina)
        {
        }

        public override void Use(GameCore<Player> game)
        {
            var effect = GetStaminaPotionEffect(Size);
            game.Player.Stamina += effect;
            game.Journal.Write(new StaminaRestoredMessage(effect));
        }

        protected override StyledLine[] GetEffectDescription()
        {
            return new[]
            {
                new StyledLine
                {
                    "Restores ",
                    new StyledString(GetStaminaPotionEffect(Size), TextHelper.StaminaColor),
                    " stamina when used."
                }
            };
        }

        protected override string GetKnownName(string sizeName)
        {
            return $"{sizeName}Stamina Potion";
        }

        private static int GetStaminaPotionEffect(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return 20;
                case PotionSize.Medium:
                    return 50;
                case PotionSize.Big:
                    return 100;
                default:
                    throw new ArgumentException($"Unknown potion size: {size}", nameof(size));
            }
        }
    }
}