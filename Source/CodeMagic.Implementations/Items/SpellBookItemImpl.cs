using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Items;
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

        public StyledString[][] GetDescription()
        {
            var result = new List<StyledString[]>
            {
                new []{new StyledString($"Weight: {Weight}") },
                new StyledString[0],
                new []{new StyledString($"Spells Capacity: {Spells.Length}") },
                new []{new StyledString($"Spells In Book: {Spells.Count(spell => spell != null)}") },
                new StyledString[0],
            };

            ItemTextHelper.AddBonusesDescription(this, result);

            result.Add(new StyledString[0]);

            result.AddRange(description.Select(line => new[] { new StyledString(line, ItemTextHelper.DescriptionTextColor) }).ToArray());

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