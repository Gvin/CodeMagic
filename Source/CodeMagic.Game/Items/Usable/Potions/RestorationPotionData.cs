using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal class RestorationPotionData : PotionData
    {
        public RestorationPotionData(PotionSize size) 
            : base(size, PotionType.Restoration)
        {
        }

        public override void Use(GameCore<Player> game)
        {
            var (restoreHealthValue, restoreManaValue, restoreStaminaValue) = GetRestorationPotionEffect(Size);

            game.Player.Stamina += restoreStaminaValue;
            game.Journal.Write(new StaminaRestoredMessage(restoreStaminaValue));
            game.Player.Mana += restoreManaValue;
            game.Journal.Write(new ManaRestoredMessage(game.Player, restoreManaValue));
            game.Player.Health += restoreHealthValue;
            game.Journal.Write(new HealedMessage(game.Player, restoreHealthValue));
        }

        protected override StyledLine[] GetEffectDescription()
        {
            var (health, mana, stamina) = GetRestorationPotionEffect(Size);
            return new[]
            {
                new StyledLine
                {
                    "Heals ",
                    new StyledString(health, TextHelper.HealthColor),
                    " health when used."
                },
                new StyledLine
                {
                    "Restores ",
                    new StyledString(mana, TextHelper.ManaColor),
                    " mana when used."
                },
                new StyledLine
                {
                    "Restores ",
                    new StyledString(stamina, TextHelper.StaminaColor),
                    " stamina when used."
                }
            };
        }

        protected override string GetKnownName(string sizeName)
        {
            return $"{sizeName}Restoration Potion";
        }

        private static (int health, int mana, int stamina) GetRestorationPotionEffect(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return (20, 100, 10);
                case PotionSize.Medium:
                    return (40, 200, 30);
                case PotionSize.Big:
                    return (60, 400, 60);
                default:
                    throw new ArgumentException($"Unknown potion size: {size}", nameof(size));
            }
        }
    }
}