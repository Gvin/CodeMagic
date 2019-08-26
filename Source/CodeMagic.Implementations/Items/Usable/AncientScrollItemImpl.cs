using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Usable
{
    public class AncientScrollItemImpl : AncientScrollItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string ImageWorld = "ItemsOnGround_Other";

        private const string ImageInventory1 = "Item_Scroll_Old_V1";
        private const string ImageInventory2 = "Item_Scroll_Old_V2";
        private const string ImageInventory3 = "Item_Scroll_Old_V3";

        private readonly string inventoryImageName;

        public AncientScrollItemImpl(AncientScrollItemConfiguration configuration) 
            : base(configuration)
        {
            inventoryImageName = GetInventoryImageName(configuration.Code);
        }

        private static string GetInventoryImageName(string code)
        {
            var letterA = code.Count(c => char.ToLower(c) == 'a');
            var letterB = code.Count(c => char.ToLower(c) == 'b');
            var letterC = code.Count(c => char.ToLower(c) == 'c');

            if (letterA > letterB && letterA > letterC)
                return ImageInventory1;
            if (letterB > letterA && letterB > letterC)
                return ImageInventory2;
            return ImageInventory3;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageWorld);
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(inventoryImageName);
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {$"Spell Name: {SpellName}"},
                new StyledLine {"Damaged: ", new StyledString($"{DamagePercent}%", ItemTextHelper.NegativeValueColor)}, 
                StyledLine.Empty,
                new StyledLine {"This scroll looks old and damaged."},
                new StyledLine {"It's title is written on some unknown language."},
                new StyledLine {"Some text can barely be red."}
            };
        }
    }
}