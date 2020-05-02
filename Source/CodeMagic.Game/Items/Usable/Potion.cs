using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable
{
    public class Potion : Item, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string SaveKeyColor = "PotionColor";
        private const string SaveKeyType = "PotionType";
        private const string SaveKeySize = "PotionSize";

        private readonly PotionColor color;
        private readonly PotionType type;
        private readonly PotionSize size;

        public Potion(SaveData data) : base(data)
        {
            color = (PotionColor) data.GetIntValue(SaveKeyColor);
            type = (PotionType) data.GetIntValue(SaveKeyType);
            size = (PotionSize) data.GetIntValue(SaveKeySize);
        }

        public Potion(PotionColor color, PotionType type, PotionSize size, ItemRareness rareness) : base(new ItemConfiguration
        {
            Key = GetKey(color),
            Name = "Potion",
            Rareness = rareness,
            Weight = GetWeight(size)
        })
        {
            this.color = color;
            this.type = type;
            this.size = size;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyType, (int) type);
            data.Add(SaveKeySize, (int) size);
            data.Add(SaveKeyColor, (int) color);
            return data;
        }

        private static int GetWeight(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return 200;
                case PotionSize.Medium:
                    return 500;
                case PotionSize.Big:
                    return 1000;
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, null);
            }
        }

        public override string Name => GetName(color, type, size);

        private static string GetName(PotionColor color, PotionType type, PotionSize size)
        {
            string sizeName;
            switch (size)
            {
                case PotionSize.Small:
                    sizeName = "Small ";
                    break;
                case PotionSize.Medium:
                    sizeName = "";
                    break;
                case PotionSize.Big:
                    sizeName = "Big ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, null);
            }

            if (((Player) CurrentGame.Player).KnownPotions.Contains(type))
            {
                switch (type)
                {
                    case PotionType.Health:
                        return $"{sizeName}Healing Potion";
                    case PotionType.Mana:
                        return $"{sizeName}Mana Potion";
                    case PotionType.Restoration:
                        return $"{sizeName}Restoration Potion";
                    case PotionType.Paralyze:
                        return $"{sizeName}Paralyze Potion";
                    case PotionType.Frost:
                        return $"{sizeName}Frost Potion";
                    case PotionType.Hunger:
                        return $"{sizeName}Hunger Potion";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }

            switch (color)
            {
                case PotionColor.Red:
                    return $"{sizeName}Red Potion";
                case PotionColor.Blue:
                    return $"{sizeName}Blue Potion";
                case PotionColor.Purple:
                    return $"{sizeName}Purple Potion";
                case PotionColor.Green:
                    return $"{sizeName}Green Potion";
                case PotionColor.Orange:
                    return $"{sizeName}Orange Potion";
                case PotionColor.Yellow:
                    return $"{sizeName}Yellow Potion";
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        private static string GetKey(PotionColor color)
        {
            switch (color)
            {
                case PotionColor.Red:
                    return "potion_red";
                case PotionColor.Blue:
                    return "potion_blue";
                case PotionColor.Purple:
                    return "potion_purple";
                case PotionColor.Green:
                    return "potion_green";
                case PotionColor.Orange:
                    return "potion_orange";
                case PotionColor.Yellow:
                    return "potion_yellow";
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        public bool Use(CurrentGame.GameCore<Player> game)
        {
            switch (type)
            {
                case PotionType.Health:
                    UseHealthPotion(game);
                    break;
                case PotionType.Mana:
                    UseManaPotion(game);
                    break;
                case PotionType.Restoration:
                    UseRestorationPotion(game);
                    break;
                case PotionType.Paralyze:
                    UseParalyzePotion(game);
                    break;
                case PotionType.Frost:
                    UseFrostPotion(game);
                    break;
                case PotionType.Hunger:
                    UseHungerPotion(game);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!game.Player.KnownPotions.Contains(type))
            {
                game.Player.KnownPotions.Add(type);
            }

            return false;
        }

        private void UseHungerPotion(CurrentGame.GameCore<Player> game)
        {
            var increase = GetHungerPotionEffect(size);
            game.Player.HungerPercent += increase;
            game.Journal.Write(new HungerIncreasedMessage(increase));
        }

        private void UseFrostPotion(CurrentGame.GameCore<Player> game)
        {
            var timeToLive = GetFrostPotionEffect(size);
            game.Player.Statuses.Add(new FrozenObjectStatus(timeToLive));
        }

        private void UseParalyzePotion(CurrentGame.GameCore<Player> game)
        {
            var timeToLive = GetParalyzePotionEffect(size);
            game.Player.Statuses.Add(new ParalyzedObjectStatus(timeToLive));
        }

        private void UseRestorationPotion(CurrentGame.GameCore<Player> game)
        {
            var (restoreHealthValue, restoreManaValue) = GetRestorationPotionEffect(size);
            game.Player.Mana += restoreManaValue;
            game.Journal.Write(new ManaRestoredMessage(game.Player, restoreManaValue));
            game.Player.Health += restoreHealthValue;
            game.Journal.Write(new HealedMessage(game.Player, restoreHealthValue));
        }

        private void UseManaPotion(CurrentGame.GameCore<Player> game)
        {
            var manaValue = GetManaPotionEffect(size);
            game.Player.Mana += manaValue;
            game.Journal.Write(new ManaRestoredMessage(game.Player, manaValue));
        }

        private void UseHealthPotion(CurrentGame.GameCore<Player> game)
        {
            var healValue = GetHealPotionEffect(size);
            game.Player.Health += healValue;
            game.Journal.Write(new HealedMessage(game.Player, healValue));
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
                    throw new ArgumentOutOfRangeException();
            }
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
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static (int health, int mana) GetRestorationPotionEffect(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return (20, 100);
                case PotionSize.Medium:
                    return (40, 200);
                case PotionSize.Big:
                    return (60, 400);
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static int GetFrostPotionEffect(PotionSize size)
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
                    throw new ArgumentOutOfRangeException();
            }
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
                    throw new ArgumentOutOfRangeException();
            }
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            switch (color)
            {
                case PotionColor.Red:
                    return storage.GetImage("ItemsOnGround_Potion_Red");
                case PotionColor.Blue:
                    return storage.GetImage("ItemsOnGround_Potion_Blue");
                case PotionColor.Purple:
                    return storage.GetImage("ItemsOnGround_Potion_Purple");
                case PotionColor.Green:
                    return storage.GetImage("ItemsOnGround_Potion_Green");
                case PotionColor.Orange:
                    return storage.GetImage("ItemsOnGround_Potion_Orange");
                case PotionColor.Yellow:
                    return storage.GetImage("ItemsOnGround_Potion_Yellow");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            switch (color)
            {
                case PotionColor.Red:
                    switch (size)
                    {
                        case PotionSize.Small:
                            return storage.GetImage("Item_Potion_Red_Small");
                        case PotionSize.Medium:
                            return storage.GetImage("Item_Potion_Red");
                        case PotionSize.Big:
                            return storage.GetImage("Item_Potion_Red_Big");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case PotionColor.Blue:
                    switch (size)
                    {
                        case PotionSize.Small:
                            return storage.GetImage("Item_Potion_Blue_Small");
                        case PotionSize.Medium:
                            return storage.GetImage("Item_Potion_Blue");
                        case PotionSize.Big:
                            return storage.GetImage("Item_Potion_Blue_Big");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case PotionColor.Purple:
                    switch (size)
                    {
                        case PotionSize.Small:
                            return storage.GetImage("Item_Potion_Purple_Small");
                        case PotionSize.Medium:
                            return storage.GetImage("Item_Potion_Purple");
                        case PotionSize.Big:
                            return storage.GetImage("Item_Potion_Purple_Big");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case PotionColor.Green:
                    switch (size)
                    {
                        case PotionSize.Small:
                            return storage.GetImage("Item_Potion_Green_Small");
                        case PotionSize.Medium:
                            return storage.GetImage("Item_Potion_Green");
                        case PotionSize.Big:
                            return storage.GetImage("Item_Potion_Green_Big");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case PotionColor.Orange:
                    switch (size)
                    {
                        case PotionSize.Small:
                            return storage.GetImage("Item_Potion_Orange_Small");
                        case PotionSize.Medium:
                            return storage.GetImage("Item_Potion_Orange");
                        case PotionSize.Big:
                            return storage.GetImage("Item_Potion_Orange_Big");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case PotionColor.Yellow:
                    switch (size)
                    {
                        case PotionSize.Small:
                            return storage.GetImage("Item_Potion_Yellow_Small");
                        case PotionSize.Medium:
                            return storage.GetImage("Item_Potion_Yellow");
                        case PotionSize.Big:
                            return storage.GetImage("Item_Potion_Yellow_Big");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public StyledLine[] GetDescription(Player player)
        {
            var result = new List<StyledLine>();

            if (player.KnownPotions.Contains(type))
            {
                result.AddRange(GetEffectDescription(type, size));
            }
            else
            {
                result.Add(new StyledLine{"???"});
            }

            result.Add(StyledLine.Empty);

            result.Add(new StyledLine {new StyledString(GetCommonDescription(color, size), TextHelper.DescriptionTextColor)});

            return result.ToArray();
        }

        private static StyledLine[] GetEffectDescription(PotionType type, PotionSize size)
        {
            switch (type)
            {
                case PotionType.Health:
                    return new[]
                    {
                        new StyledLine
                        {
                            "Heals ",
                            new StyledString(GetHealPotionEffect(size).ToString(), TextHelper.HealthColor),
                            " health when used."
                        }
                    };
                case PotionType.Mana:
                    return new[]
                    {
                        new StyledLine
                        {
                            "Restores ",
                            new StyledString(GetManaPotionEffect(size).ToString(), TextHelper.ManaColor),
                            " mana when used."
                        }
                    };
                case PotionType.Restoration:
                    var (health, mana) = GetRestorationPotionEffect(size);
                    return new[]
                    {
                        new StyledLine
                        {
                            "Heals ",
                            new StyledString(health.ToString(), TextHelper.HealthColor),
                            " health when used."
                        },
                        new StyledLine
                        {
                            "Restores ",
                            new StyledString(mana.ToString(), TextHelper.ManaColor),
                            " mana when used."
                        }
                    };
                case PotionType.Paralyze:
                    return new[]
                    {
                        new StyledLine
                        {
                            $"Paralyzes target for {GetParalyzePotionEffect(size)} turns when used."
                        }
                    };
                case PotionType.Frost:
                    return new[]
                    {
                        new StyledLine
                        {
                            $"Freezes target for {GetFrostPotionEffect(size)} turns when used."
                        }
                    };
                case PotionType.Hunger:
                    return new[]
                    {
                        new StyledLine
                        {
                            $"Increases hunger for {GetHungerPotionEffect(size)}% when used."
                        }
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static string GetCommonDescription(PotionColor color, PotionSize size)
        {
            var sizeDescription = GetSizeDescription(size);

            switch (color)
            {
                case PotionColor.Red:
                    return $"{sizeDescription} with bright red liquid.";
                case PotionColor.Blue:
                    return $"{sizeDescription} with bright blue liquid.";
                case PotionColor.Purple:
                    return $"{sizeDescription} with bright purple liquid.";
                case PotionColor.Green:
                    return $"{sizeDescription} with bright green liquid.";
                case PotionColor.Orange:
                    return $"{sizeDescription} with bright orange liquid.";
                case PotionColor.Yellow:
                    return $"{sizeDescription} with bright yellow liquid.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
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
    }

    public enum PotionSize
    {
        Small,
        Medium,
        Big
    }

    public enum PotionColor
    {
        Red,
        Blue,
        Purple,
        Green,
        Orange,
        Yellow
    }

    public enum PotionType
    {
        Health,
        Mana,
        Restoration,
        Paralyze,
        Frost,
        Hunger
    }
}