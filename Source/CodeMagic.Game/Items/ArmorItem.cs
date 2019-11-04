using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public class ArmorItem : EquipableItem, IInventoryImageProvider, IDescriptionProvider, IWorldImageProvider
    {
        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage worldImage;
        private readonly string[] description;
        private readonly Dictionary<Element, int> protection;

        public ArmorItem(ArmorItemConfiguration configuration) 
            : base(configuration)
        {
            protection = configuration.Protection.ToDictionary(pair => pair.Key, pair => pair.Value);
            ArmorType = configuration.ArmorType;
            inventoryImage = configuration.InventoryImage;
            worldImage = configuration.WorldImage;
            description = configuration.Description;
        }

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
                result.Add(ItemTextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(ItemTextHelper.GetCompareWeightLine(Weight, equipedArmor.Weight));
            }

            result.Add(StyledLine.Empty);

            AddProtectionDescription(result, equipedArmor);

            result.Add(StyledLine.Empty);

            ItemTextHelper.AddBonusesDescription(this, equipedArmor, result);

            result.Add(StyledLine.Empty);

            result.AddRange(ItemTextHelper.ConvertDescription(description));

            return result.ToArray();
        }

        private void AddProtectionDescription(List<StyledLine> descriptionResult, ArmorItem equipedArmor)
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
                        new StyledString($"{ItemTextHelper.GetElementName(element)}",
                            ItemTextHelper.GetElementColor(element)),
                        " Protection: "
                    };

                    if (equiped)
                    {
                        protectionLine.Add(ItemTextHelper.GetValueString(value, "%"));
                    }
                    else
                    {
                        protectionLine.Add(ItemTextHelper.GetCompareValueString(value, equipedValue, "%"));
                    }

                    descriptionResult.Add(protectionLine);
                }
            }
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return worldImage;
        }
    }

    public enum ArmorType
    {
        Helmet = 0,
        Chest = 1,
        Leggings = 2
    }

    public class ArmorItemConfiguration : EquipableItemConfiguration
    {
        public ArmorItemConfiguration()
        {
            Protection = new Dictionary<Element, int>();
        }

        public SymbolsImage InventoryImage { get; set; }

        public SymbolsImage WorldImage { get; set; }

        public string[] Description { get; set; }

        public Dictionary<Element, int> Protection { get; set; }

        public ArmorType ArmorType { get; set; }
    }
}