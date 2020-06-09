using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable.Potions
{
    public sealed class Potion : Item, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private static readonly IPotionDataFactory DataFactory = new PotionDataFactory();

        private const string SaveKeyColor = "PotionColor";
        private const string SaveKeyType = "PotionType";
        private const string SaveKeySize = "PotionSize";

        private readonly PotionColor color;
        private readonly PotionType type;
        private readonly PotionSize size;
        private readonly PotionData potionData;

        public Potion(SaveData data) : base(data)
        {
            color = (PotionColor) data.GetIntValue(SaveKeyColor);
            type = (PotionType) data.GetIntValue(SaveKeyType);
            size = (PotionSize) data.GetIntValue(SaveKeySize);

            potionData = DataFactory.GetPotionData(type, size);
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

            potionData = DataFactory.GetPotionData(type, size);
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

        public override string Name => potionData.GetName(color);

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
                case PotionColor.White:
                    return "potion_white";
                case PotionColor.Gray:
                    return "potion_gray";
                default:
                    throw new ArgumentException($"Unknown potion color: {color}", nameof(color));
            }
        }

        public bool Use(GameCore<Player> game)
        {
            potionData.Use(game);

            if (!game.Player.KnownPotions.Contains(type))
            {
                game.Player.KnownPotions.Add(type);
            }

            return false;
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
                case PotionColor.White:
                    return storage.GetImage("ItemsOnGround_Potion_White");
                case PotionColor.Gray:
                    return storage.GetImage("ItemsOnGround_Potion_Gray");
                default:
                    throw new ArgumentException($"Unknown potion color: {color}");
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
                            throw new ArgumentException($"Unknown potion size: {size}");
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
                            throw new ArgumentException($"Unknown potion size: {size}");
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
                            throw new ArgumentException($"Unknown potion size: {size}");
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
                            throw new ArgumentException($"Unknown potion size: {size}");
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
                            throw new ArgumentException($"Unknown potion size: {size}");
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
                            throw new ArgumentException($"Unknown potion size: {size}");
                    }
                case PotionColor.White:
                    switch (size)
                    {
                        case PotionSize.Small:
                            return storage.GetImage("Item_Potion_White_Small");
                        case PotionSize.Medium:
                            return storage.GetImage("Item_Potion_White");
                        case PotionSize.Big:
                            return storage.GetImage("Item_Potion_White_Big");
                        default:
                            throw new ArgumentException($"Unknown potion size: {size}");
                    }
                case PotionColor.Gray:
                    switch (size)
                    {
                        case PotionSize.Small:
                            return storage.GetImage("Item_Potion_Gray_Small");
                        case PotionSize.Medium:
                            return storage.GetImage("Item_Potion_Gray");
                        case PotionSize.Big:
                            return storage.GetImage("Item_Potion_Gray_Big");
                        default:
                            throw new ArgumentException($"Unknown potion size: {size}");
                    }
                default:
                    throw new ArgumentException($"Unknown potion color: {color}");
            }
        }

        public StyledLine[] GetDescription(Player player)
        {
            return potionData.GetDescription(player, color);
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
        Yellow,
        White,
        Gray
    }

    public enum PotionType
    {
        Health,
        Mana,
        Restoration,
        Stamina,
        Paralyze,
        Freeze,
        Hunger,
        Blind
    }
}