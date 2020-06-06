using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public class TorchItem : WeaponItem
    {
        private const string InventoryImage = "Weapon_Torch";
        private const string WorldImage = "ItemsOnGround_Torch";
        private const string EquippedImageRight = "ItemOnPlayer_Weapon_Right_Torch";
        private const string EquippedImageLeft = "ItemOnPlayer_Weapon_Left_Torch";

        private readonly AnimationsBatchManager animation;

        public TorchItem(SaveData data) : base(data)
        {
            animation = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500),
                AnimationFrameStrategy.OneByOneStartFromRandom);
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
            Description = new []{ "Rude torch made from wood and clothes." },
            MaxDurability = 20
        })
        {
            animation = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500),
                AnimationFrameStrategy.OneByOneStartFromRandom);
        }

        public override SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(InventoryImage);
        }

        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImage);
        }

        protected override SymbolsImage GetRightEquippedImage(IImagesStorage storage)
        {
            return animation.GetImage(storage, EquippedImageRight);
        }

        protected override SymbolsImage GetLeftEquippedImage(IImagesStorage storage)
        {
            return animation.GetImage(storage, EquippedImageLeft);
        }
    }
}