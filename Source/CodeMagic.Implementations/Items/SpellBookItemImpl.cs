using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items
{
    public class SpellBookItemImpl : SpellBook, IInventoryImageProvider, IDescriptionProvider, IWorldImageProvider
    {
        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage worldImage;
        private readonly string[] description;

        public SpellBookItemImpl(SpellBookItemImplConfiguration configuration) 
            : base(configuration)
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
            var equipedBook = player.Equipment.SpellBook;

            var result = new List<StyledLine>();

            if (equipedBook == null || Equals(equipedBook))
            {
                result.Add(ItemTextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(ItemTextHelper.GetCompareWeightLine(Weight, equipedBook.Weight));
            }

            result.Add(StyledLine.Empty);

            var capacityLine = new StyledLine { "Spells Capacity: " };
            if (equipedBook == null || Equals(equipedBook))
            {
                capacityLine.Add(ItemTextHelper.GetValueString(BookSize, formatBonus: false));
            }
            else
            {
                capacityLine.Add(ItemTextHelper.GetCompareValueString(BookSize, equipedBook.BookSize, formatBonus: false));
            }
            result.Add(capacityLine);

            result.Add(new StyledLine { $"Spells In Book: {Spells.Count(spell => spell != null)}" });

            result.Add(StyledLine.Empty);

            result.AddRange(ItemTextHelper.ConvertDescription(description));

            return result.ToArray();
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return worldImage;
        }
    }

    public class SpellBookItemImplConfiguration : SpellBookConfiguration
    {
        public SymbolsImage InventoryImage { get; set; }

        public SymbolsImage WorldImage { get; set; }

        public string[] Description { get; set; }
    }
}