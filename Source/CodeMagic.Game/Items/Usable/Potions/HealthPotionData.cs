using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal class HealthPotionData : PotionData
    {
        public HealthPotionData(PotionSize size) 
            : base(size, PotionType.Health)
        {
        }

        public override void Use(GameCore<Player> game)
        {
            var healValue = GetHealPotionEffect(Size);
            game.Player.Health += healValue;
            game.Journal.Write(new HealedMessage(game.Player, healValue));
        }

        protected override StyledLine[] GetEffectDescription()
        {
            return new[]
            {
                new StyledLine
                {
                    "Heals ",
                    new StyledString(GetHealPotionEffect(Size).ToString(), TextHelper.HealthColor),
                    " health when used."
                }
            };
        }

        protected override string GetKnownName(string sizeName)
        {
            return $"{sizeName}Healing Potion";
        }

        private static int GetHealPotionEffect(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return 25;
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