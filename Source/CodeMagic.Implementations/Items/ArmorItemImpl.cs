using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
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

        public StyledString[][] GetDescription()
        {
            var result = new List<StyledString[]>
            {
                new[] {new StyledString($"Weight: {Weight}")},
                new StyledString[0]
            };

            AddProtectionDescription(result);

            result.Add(new StyledString[0]);

            ItemTextHelper.AddBonusesDescription(this, result);

            result.Add(new StyledString[0]);

            result.AddRange(description.Select(line => new[] { new StyledString(line, ItemTextHelper.DescriptionTextColor) }).ToArray());

            return result.ToArray();
        }

        private void AddProtectionDescription(List<StyledString[]> descriptionResult)
        {
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var value = GetProtection(element);
                if (value != 0)
                {
                    descriptionResult.Add(new[]
                    {
                        new StyledString($"{ItemTextHelper.GetElementName(element)}", ItemTextHelper.GetElementColor(element)), 
                        new StyledString(" Protection: "),
                        new StyledString(ItemTextHelper.FormatBonusNumber(value), value > 0 ? ItemTextHelper.PositiveValueColor : ItemTextHelper.NegativeValueColor), 
                        new StyledString("%")
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