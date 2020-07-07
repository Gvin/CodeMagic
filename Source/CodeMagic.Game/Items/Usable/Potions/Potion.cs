using System;
using System.Collections.Generic;
using System.Drawing;
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
            var templateImage = storage.GetImage("ItemsOnGround_Potion");
            var palette = GetPotionPalette();
            return SymbolsImage.Recolor(templateImage, palette);
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            var imageTemplateName = GetInventoryImageTemplateName();
            var templateImage = storage.GetImage(imageTemplateName);
            var palette = GetPotionPalette();
            return SymbolsImage.Recolor(templateImage, palette);
        }

        private Dictionary<Color, Color> GetPotionPalette()
        {
            switch (color)
            {
                case PotionColor.Red:
                    return new Dictionary<Color, Color>
                    {
                        {Color.FromArgb(255, 0, 0), Color.FromArgb(255, 0, 0)},
                        {Color.FromArgb(0, 255, 0), Color.FromArgb(196, 0, 0)}
                    };
                case PotionColor.Blue:
                    return new Dictionary<Color, Color>
                    {
                        {Color.FromArgb(255, 0, 0), Color.FromArgb(79, 79, 255)},
                        {Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 255)}
                    };
                case PotionColor.Purple:
                    return new Dictionary<Color, Color>
                    {
                        {Color.FromArgb(255, 0, 0), Color.FromArgb(230, 0, 230)},
                        {Color.FromArgb(0, 255, 0), Color.FromArgb(128, 0, 128)}
                    };
                case PotionColor.Green:
                    return new Dictionary<Color, Color>
                    {
                        {Color.FromArgb(255, 0, 0), Color.FromArgb(0, 210, 0)},
                        {Color.FromArgb(0, 255, 0), Color.FromArgb(0, 128, 0)}
                    };
                case PotionColor.Orange:
                    return new Dictionary<Color, Color>
                    {
                        {Color.FromArgb(255, 0, 0), Color.FromArgb(255, 128, 0)},
                        {Color.FromArgb(0, 255, 0), Color.FromArgb(150, 70, 0)}
                    };
                case PotionColor.Yellow:
                    return new Dictionary<Color, Color>
                    {
                        {Color.FromArgb(255, 0, 0), Color.FromArgb(255, 255, 0)},
                        {Color.FromArgb(0, 255, 0), Color.FromArgb(150, 150, 0)}
                    };
                case PotionColor.White:
                    return new Dictionary<Color, Color>
                    {
                        {Color.FromArgb(255, 0, 0), Color.FromArgb(255, 255, 255)},
                        {Color.FromArgb(0, 255, 0), Color.FromArgb(192, 192, 192)}
                    };
                case PotionColor.Gray:
                    return new Dictionary<Color, Color>
                    {
                        {Color.FromArgb(255, 0, 0), Color.FromArgb(192, 192, 192)},
                        {Color.FromArgb(0, 255, 0), Color.FromArgb(128, 128, 128)}
                    };
                default:
                    throw new ArgumentException($"Unknown potion color: {color}");
            }
        }

        private string GetInventoryImageTemplateName()
        {
            switch (size)
            {
                case PotionSize.Small:
                    return "Item_Potion_Small";
                case PotionSize.Medium:
                    return "Item_Potion";
                case PotionSize.Big:
                    return "Item_Potion_Big";
                default:
                    throw new ArgumentException($"Unknown potion size: {size}");
            }
        }

        public StyledLine[] GetDescription(Player player)
        {
            var description = new List<StyledLine>
            {
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty
            };
            description.AddRange(potionData.GetDescription(player, color));

            return description.ToArray();
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