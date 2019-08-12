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
            var result = new List<StyledLine>
            {
                new StyledLine {$"Weight: {Weight}"},
                StyledLine.Empty,
                new StyledLine
                {
                    $"Spells Capacity: {Size}",
                    ItemTextHelper.GetComparisonString(Size, player.Equipment.SpellBook?.Size ?? 0)
                },
                new StyledLine { $"Spells In Book: {Spells.Count(spell => spell != null)}" },
                StyledLine.Empty,
            };

            result.AddRange(description.Select(line => new StyledLine
            {
                new StyledString(line, ItemTextHelper.DescriptionTextColor)
            }).ToArray());

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