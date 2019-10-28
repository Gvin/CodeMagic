using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public class SpellBook : EquipableItem, IInventoryImageProvider, IDescriptionProvider, IWorldImageProvider
    {
        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage worldImage;
        private readonly string[] description;

        public SpellBook(SpellBookConfiguration configuration) 
            : base(configuration)
        {
            Spells = new BookSpell[configuration.Size];
            inventoryImage = configuration.InventoryImage;
            worldImage = configuration.WorldImage;
            description = configuration.Description;
        }

        public BookSpell[] Spells { get; }

        public int BookSize => Spells.Length;

        public override bool Stackable => false;

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return inventoryImage;
        }

        public StyledLine[] GetDescription(Player player)
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

    public class SpellBookConfiguration : EquipableItemConfiguration
    {
        public int Size { get; set; }

        public SymbolsImage InventoryImage { get; set; }

        public SymbolsImage WorldImage { get; set; }

        public string[] Description { get; set; }
    }
}