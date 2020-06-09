using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal class ParalyzePotionData : PotionData
    {
        public ParalyzePotionData(PotionSize size) : base(size, PotionType.Paralyze)
        {
        }

        public override void Use(GameCore<Player> game)
        {
            var timeToLive = GetParalyzePotionEffect(Size);
            game.Player.Statuses.Add(new ParalyzedObjectStatus(timeToLive));
        }

        protected override StyledLine[] GetEffectDescription()
        {
            return new[]
            {
                new StyledLine
                {
                    $"Paralyzes target for {GetParalyzePotionEffect(Size)} turns when used."
                }
            };
        }

        protected override string GetKnownName(string sizeName)
        {
            return $"{sizeName}Paralyze Potion";
        }

        private static int GetParalyzePotionEffect(PotionSize size)
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