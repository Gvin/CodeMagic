using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal class HungerPotionData : PotionData
    {
        public HungerPotionData(PotionSize size) 
            : base(size, PotionType.Hunger)
        {
        }

        public override void Use(GameCore<Player> game)
        {
            var increase = GetHungerPotionEffect(Size);
            game.Player.HungerPercent += increase;
            game.Journal.Write(new HungerIncreasedMessage(increase));
        }

        protected override StyledLine[] GetEffectDescription()
        {
            return new[]
            {
                new StyledLine
                {
                    $"Increases hunger for {GetHungerPotionEffect(Size)}% when used."
                }
            };
        }

        protected override string GetKnownName(string sizeName)
        {
            return $"{sizeName}Hunger Potion";
        }

        private static int GetHungerPotionEffect(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return 10;
                case PotionSize.Medium:
                    return 20;
                case PotionSize.Big:
                    return 30;
                default:
                    throw new ArgumentException($"Unknown potion size: {size}", nameof(size));
            }
        }
    }
}