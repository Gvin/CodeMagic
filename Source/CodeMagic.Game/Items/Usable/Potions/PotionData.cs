using System;
using System.Collections.Generic;
using System.Globalization;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal abstract class PotionData
    {
        protected readonly PotionSize Size;
        private readonly PotionType type;

        protected PotionData(PotionSize size, PotionType type)
        {
            Size = size;
            this.type = type;
        }

        public string GetName(PotionColor color)
        {
            var sizeName = GetSizeName();
            var knownPotion = ((Player) CurrentGame.Player).KnownPotions.Contains(type);

            if (knownPotion)
            {
                return GetKnownName(sizeName);
            }

            var colorName = GetColorName(color);
            colorName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(colorName);
            return $"{sizeName}{colorName} Potion";
        }

        public abstract void Use(GameCore<Player> game);

        public StyledLine[] GetDescription(Player player, PotionColor color)
        {
            var result = new List<StyledLine>();

            if (player.KnownPotions.Contains(type))
            {
                result.AddRange(GetEffectDescription());
            }
            else
            {
                result.Add(new StyledLine { "???" });
            }

            result.Add(StyledLine.Empty);

            result.Add(new StyledLine { new StyledString(GetCommonDescription(color, Size), TextHelper.DescriptionTextColor) });

            return result.ToArray();
        }

        private static string GetCommonDescription(PotionColor color, PotionSize size)
        {
            var sizeDescription = GetSizeDescription(size);
            var colorName = GetColorName(color);
            return $"{sizeDescription} with {colorName} liquid.";
        }

        private static string GetSizeDescription(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return "A small phial with";
                case PotionSize.Medium:
                    return "Medium size jar with";
                case PotionSize.Big:
                    return "Big jar with";
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, null);
            }
        }

        protected abstract StyledLine[] GetEffectDescription();

        private static string GetColorName(PotionColor color)
        {
            return color.ToString().ToLower();
        }

        private string GetSizeName()
        {
            switch (Size)
            {
                case PotionSize.Small:
                    return "Small ";
                case PotionSize.Medium:
                    return "";
                case PotionSize.Big:
                    return "Big ";
                default:
                    throw new ArgumentException($"Unknown potion size: {Size}");
            }
        }

        protected abstract string GetKnownName(string sizeName);
    }
}