using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Saving;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public class SpellBook : EquipableItem, IInventoryImageProvider, IDescriptionProvider, IWorldImageProvider
    {
        private const string SaveKeyInventoryImage = "InventoryImage";
        private const string SaveKeyWorldImage = "WorldImage";
        private const string SaveKeySpells = "Spells";
        private const string SaveKeySize = "Size";

        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage worldImage;

        public SpellBook(SaveData data) : base(data)
        {
            BookSize = data.GetIntValue(SaveKeySize);
            var spellsRaw = data.GetObjectsCollection<BookSpell>(SaveKeySpells);
            Spells = new BookSpell[BookSize];
            for (int index = 0; index < spellsRaw.Length; index++)
            {
                Spells[index] = spellsRaw[index];
            }

            inventoryImage = data.GetObject<SymbolsImageSaveable>(SaveKeyInventoryImage).GetImage();
            worldImage = data.GetObject<SymbolsImageSaveable>(SaveKeyWorldImage).GetImage();
        }

        public SpellBook(SpellBookConfiguration configuration) 
            : base(configuration)
        {
            BookSize = configuration.Size;
            Spells = new BookSpell[BookSize];
            inventoryImage = configuration.InventoryImage;
            worldImage = configuration.WorldImage;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyInventoryImage, new SymbolsImageSaveable(inventoryImage));
            data.Add(SaveKeyWorldImage, new SymbolsImageSaveable(worldImage));
            data.Add(SaveKeySize, BookSize);
            data.Add(SaveKeySpells, Spells.Where(spell => spell != null).ToArray());
            return data;
        }

        public BookSpell[] Spells { get; }

        public int BookSize { get; }

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
                result.Add(TextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(TextHelper.GetCompareWeightLine(Weight, equipedBook.Weight));
            }

            result.Add(StyledLine.Empty);

            var capacityLine = new StyledLine { "Spells Capacity: " };
            if (equipedBook == null || Equals(equipedBook))
            {
                capacityLine.Add(TextHelper.GetValueString(BookSize, formatBonus: false));
            }
            else
            {
                capacityLine.Add(TextHelper.GetCompareValueString(BookSize, equipedBook.BookSize, formatBonus: false));
            }
            result.Add(capacityLine);

            result.Add(new StyledLine { $"Spells In Book: {Spells.Count(spell => spell != null)}" });

            result.Add(StyledLine.Empty);
            TextHelper.AddBonusesDescription(this, equipedBook, result);

            result.Add(StyledLine.Empty);
            TextHelper.AddLightBonusDescription(this, result);

            result.Add(StyledLine.Empty);

            result.AddRange(TextHelper.ConvertDescription(Description));

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
    }
}