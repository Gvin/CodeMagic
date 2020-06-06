using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public class ArmorItem : DurableItem, IArmorItem, IInventoryImageProvider, IDescriptionProvider, IWorldImageProvider, IEquippedImageProvider
    {
        private const string SaveKeyInventoryImage = "InventoryImage";
        private const string SaveKeyWorldImage = "WorldImage";
        private const string SaveKeyEquippedImage = "EquippedImage";
        private const string SaveKeyProtection = "Protection";
        private const string SaveKetArmorType = "ArmorType";

        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage worldImage;
        private readonly SymbolsImage equippedImage;
        private readonly Dictionary<Element, int> protection;

        public ArmorItem(SaveData data) : base(data)
        {
            inventoryImage = data.GetObject<SymbolsImageSaveable>(SaveKeyInventoryImage)?.GetImage();
            worldImage = data.GetObject<SymbolsImageSaveable>(SaveKeyWorldImage)?.GetImage();
            equippedImage = data.GetObject<SymbolsImageSaveable>(SaveKeyEquippedImage)?.GetImage();

            protection = data.GetObject<DictionarySaveable>(SaveKeyProtection).Data.ToDictionary(pair =>
                (Element) int.Parse((string) pair.Key), pair => int.Parse((string) pair.Value));
            ArmorType = (ArmorType) data.GetIntValue(SaveKetArmorType);
        }

        public ArmorItem(ArmorItemConfiguration configuration) 
            : base(configuration)
        {
            protection = configuration.Protection.ToDictionary(pair => pair.Key, pair => pair.Value);
            ArmorType = configuration.ArmorType;

            inventoryImage = configuration.InventoryImage;
            worldImage = configuration.WorldImage;
            equippedImage = configuration.EquippedImage;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyInventoryImage, inventoryImage !=null ? new SymbolsImageSaveable(inventoryImage) : null);
            data.Add(SaveKeyWorldImage, worldImage != null ? new SymbolsImageSaveable(worldImage) : null);
            data.Add(SaveKeyEquippedImage, equippedImage != null ? new SymbolsImageSaveable(equippedImage) : null);
            data.Add(SaveKetArmorType, (int) ArmorType);
            data.Add(SaveKeyProtection,
                new DictionarySaveable(protection.ToDictionary(pair => (object) (int) pair.Key,
                    pair => (object) pair.Value)));
            return data;
        }

        public int EquippedImageOrder => (int) ArmorType;

        public ArmorType ArmorType { get; }

        public int GetProtection(Element element)
        {
            return protection.ContainsKey(element) ? protection[element] : 0;
        }

        public override bool Stackable => false;

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return inventoryImage;
        }

        public StyledLine[] GetDescription(Player player)
        {
            var equipedArmor = player.Equipment.Armor[ArmorType];

            var result = new List<StyledLine>();

            if (equipedArmor == null || Equals(equipedArmor))
            {
                result.Add(TextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(TextHelper.GetCompareWeightLine(Weight, equipedArmor.Weight));
            }

            result.Add(StyledLine.Empty);

            result.Add(TextHelper.GetDurabilityLine(Durability, MaxDurability));

            result.Add(StyledLine.Empty);

            AddProtectionDescription(result, equipedArmor);

            result.Add(StyledLine.Empty);

            TextHelper.AddBonusesDescription(this, equipedArmor, result);

            result.Add(StyledLine.Empty);

            result.AddRange(TextHelper.ConvertDescription(Description));

            return result.ToArray();
        }

        private void AddProtectionDescription(List<StyledLine> descriptionResult, IArmorItem equipedArmor)
        {
            var equiped = Equals(equipedArmor);
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var value = GetProtection(element);
                var equipedValue = equipedArmor?.GetProtection(element) ?? 0;

                if (value != 0 || equipedValue != 0)
                {
                    var protectionLine = new StyledLine
                    {
                        new StyledString($"{TextHelper.GetElementName(element)}",
                            TextHelper.GetElementColor(element)),
                        " Protection: "
                    };

                    if (equiped)
                    {
                        protectionLine.Add(TextHelper.GetValueString(value, "%"));
                    }
                    else
                    {
                        protectionLine.Add(TextHelper.GetCompareValueString(value, equipedValue, "%"));
                    }

                    descriptionResult.Add(protectionLine);
                }
            }
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return worldImage;
        }

        public SymbolsImage GetEquippedImage(Player player, IImagesStorage imagesStorage)
        {
            return equippedImage;
        }
    }

    public enum ArmorType
    {
        Helmet = 0,
        Chest = 1,
        Leggings = 2
    }

    public class ArmorItemConfiguration : DurableItemConfiguration
    {
        public ArmorItemConfiguration()
        {
            Protection = new Dictionary<Element, int>();
        }

        public SymbolsImage InventoryImage { get; set; }

        public SymbolsImage WorldImage { get; set; }

        public SymbolsImage EquippedImage { get; set; }

        public Dictionary<Element, int> Protection { get; set; }

        public ArmorType ArmorType { get; set; }
    }
}