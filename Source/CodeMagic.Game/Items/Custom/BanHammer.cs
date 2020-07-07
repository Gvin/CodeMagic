using System.Collections.Generic;
using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Custom
{
    public class BanHammer : WeaponItem
    {
        public BanHammer(SaveData data) 
            : base(data)
        {
        }

        public BanHammer() : base(new WeaponItemConfiguration
        {
            Name = "Ban Hammer",
            MaxDurability = 100000000,
            Description = new[]
            {
                "Powerful weapon designed to give his owner",
                "the power of God. For testing purpose mostly."
            },
            HitChance = 100,
            Key = "weapon_ban_hammer",
            Weight = 0,
            LightPower = LightLevel.Medium,
            Rareness = ItemRareness.Epic,
            MinDamage = new Dictionary<Element, int>
            {
                {Element.Acid, 100},
                {Element.Blunt, 100},
                {Element.Electricity, 100},
                {Element.Fire, 100},
                {Element.Frost, 100},
                {Element.Piercing, 100},
                {Element.Slashing, 100},
                {Element.Magic, 100}
            },
            MaxDamage = new Dictionary<Element, int>
            {
                {Element.Acid, 200},
                {Element.Blunt, 200},
                {Element.Electricity, 200},
                {Element.Fire, 200},
                {Element.Frost, 200},
                {Element.Piercing, 200},
                {Element.Slashing, 200},
                {Element.Magic, 200}
            }
        })
        {
        }

        public override SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Weapon_BanHammer");
        }

        protected override SymbolsImage GetLeftEquippedImage(IImagesStorage storage)
        {
            return RecolorImage(storage.GetImage("ItemOnPlayer_Weapon_Left_Mace"));
        }

        protected override SymbolsImage GetRightEquippedImage(IImagesStorage storage)
        {
            return RecolorImage(storage.GetImage("ItemOnPlayer_Weapon_Right_Mace"));
        }

        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return RecolorImage(storage.GetImage("ItemsOnGround_Weapon_Mace"));
        }

        private SymbolsImage RecolorImage(SymbolsImage image)
        {
            var palette = new Dictionary<Color, Color>
            {
                {Color.FromArgb(255, 0, 0), Color.MediumVioletRed}
            };
            return SymbolsImage.Recolor(image, palette);
        }
    }
}