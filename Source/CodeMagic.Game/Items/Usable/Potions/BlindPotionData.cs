using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal class BlindPotionData : PotionData
    {
        public BlindPotionData(PotionSize size) : base(size, PotionType.Blind)
        {
        }

        public override void Use(GameCore<Player> game)
        {
            var timeToLive = GetBlindPotionEffect(Size);
            game.Player.Statuses.Add(new BlindObjectStatus(timeToLive));
        }

        protected override StyledLine[] GetEffectDescription()
        {
            return new[]
            {
                new StyledLine
                {
                    $"Blinds target for {GetBlindPotionEffect(Size)} turns when used."
                }
            };
        }

        protected override string GetKnownName(string sizeName)
        {
            return $"{sizeName}Blind Potion";
        }

        private static int GetBlindPotionEffect(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return 2;
                case PotionSize.Medium:
                    return 4;
                case PotionSize.Big:
                    return 8;
                default:
                    throw new ArgumentException($"Unknown potion size: {size}", nameof(size));
            }
        }
    }
}