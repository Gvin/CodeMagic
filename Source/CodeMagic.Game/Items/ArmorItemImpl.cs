using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public class ArmorItemImpl : ArmorItem, IInventoryImageProvider, IDescriptionProvider, IWorldImageProvider
    {
        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage worldImage;
        private readonly string[] description;

        public ArmorItemImpl(ArmorItemImplConfiguration configuration) : base(configuration)
        {
            inventoryImage = configuration.InventoryImage;
            worldImage = configuration.WorldImage;
            description = configuration.Description;
        }

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

    public class ArmorItemImplConfiguration : ArmorItemConfiguration
    {
        public SymbolsImage InventoryImage { get; set; }

        public SymbolsImage WorldImage { get; set; }

        public string[] Description { get; set; }
    }
}