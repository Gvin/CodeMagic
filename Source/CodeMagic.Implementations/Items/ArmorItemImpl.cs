using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items
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

        public StyledLine[] GetDescription(IPlayer player)
        {
            var equipedArmor = player.Equipment.Armor[ArmorType];
            var result = new List<StyledLine>
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty
            };

            AddProtectionDescription(result, equipedArmor);

            result.Add(StyledLine.Empty);


            ItemTextHelper.AddBonusesDescription(this, equipedArmor, result);

            result.Add(StyledLine.Empty);

            result.AddRange(description.Select(line => new StyledLine
            {
                new StyledString(line, ItemTextHelper.DescriptionTextColor)
            }).ToArray());

            return result.ToArray();
        }

        private void AddProtectionDescription(List<StyledLine> descriptionResult, ArmorItem equipedArmor)
        {
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var value = GetProtection(element);
                var equipedValue = equipedArmor?.GetProtection(element) ?? 0;

                if (value != 0 || equipedValue != 0)
                {
                    descriptionResult.Add(new StyledLine
                    {
                        new StyledString($"{ItemTextHelper.GetElementName(element)}", ItemTextHelper.GetElementColor(element)), 
                        " Protection: ",
                        new StyledString(ItemTextHelper.FormatBonusNumber(value), value > 0 ? ItemTextHelper.PositiveValueColor : ItemTextHelper.NegativeValueColor), 
                        "%",
                        ItemTextHelper.GetComparisonString(value, equipedValue)
                    });
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