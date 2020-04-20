using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public class TorchItem : WeaponItem
    {
        private const string InventoryImage = "Weapon_Torch";
        private const string WorldImage = "ItemsOnGround_Torch";

        public TorchItem(SaveData data) : base(data)
        {
        }

        public TorchItem() : base(new WeaponItemConfiguration
        {
            Name = "Torch",
            Key = "torch",
            HitChance = 50,
            Rareness = ItemRareness.Trash,
            Weight = 1500,
            MinDamage = new Dictionary<Element, int>
            {
                {Element.Fire, 1},
                {Element.Blunt, 1}
            },
            MaxDamage = new Dictionary<Element, int>
            {
                {Element.Fire, 3},
                {Element.Blunt, 3}
            },
            LightPower = LightLevel.Dim1,
            Description = new []{ "Rude torch made from wood and clothes." }
        })
        {
        }

        public override SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(InventoryImage);
        }

        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImage);
        }
    }
}