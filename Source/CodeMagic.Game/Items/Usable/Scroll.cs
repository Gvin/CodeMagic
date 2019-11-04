using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable
{
    public class Scroll : ScrollBase
    {
        private const string ImageWorld = "ItemsOnGround_Other";

        private const string ImageInventory1 = "Item_Scroll_New_V1";
        private const string ImageInventory2 = "Item_Scroll_New_V2";
        private const string ImageInventory3 = "Item_Scroll_New_V3";

        private readonly string inventoryImageName;

        public Scroll(ScrollItemConfiguration configuration) 
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

        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageWorld);
        }

        public override SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(inventoryImageName);
        }

        public override StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {$"Spell Name: {SpellName}"},
                new StyledLine {"Spell Mana: ", new StyledString(Mana.ToString(), ItemTextHelper.ManaColor)}, 
                StyledLine.Empty, 
                new StyledLine {"A new scroll that you have created. A single use item."},
                new StyledLine {"It can be used to cast a spell without mana loss."}
            };
        }
    }

    public class ScrollItemConfiguration : ItemConfiguration
    {
        public ScrollItemConfiguration()
        {
            Weight = 300;
        }

        public string SpellName { get; set; }

        public string Code { get; set; }

        public int Mana { get; set; }
    }
}