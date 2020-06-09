using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal class FreezePotionData : PotionData
    {
        public FreezePotionData(PotionSize size) : base(size, PotionType.Freeze)
        {
        }

        public override void Use(GameCore<Player> game)
        {
            var timeToLive = GetFreezePotionEffect(Size);
            game.Player.Statuses.Add(new FrozenObjectStatus(timeToLive));
        }

        protected override StyledLine[] GetEffectDescription()
        {
            return new[]
            {
                new StyledLine
                {
                    $"Freezes target for {GetFreezePotionEffect(Size)} turns when used."
                }
            };
        }

        protected override string GetKnownName(string sizeName)
        {
            return $"{sizeName}Freeze Potion";
        }

        private static int GetFreezePotionEffect(PotionSize size)
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
                    throw new ArgumentException($"Unknown potion type: {size}", nameof(size));
            }
        }
    }
}