using System.Collections.Generic;
using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items
{
    public class TorchItem : WeaponItem, IInventoryImageProvider, IWorldImageProvider, IDescriptionProvider, ILightSource, ILightObject
    {
        private const string InventoryImage = "Weapon_Torch";
        private const string WorldImage = "ItemsOnGround_Torch";

        public TorchItem() : base(new WeaponItemConfiguration
        {
            Name = "Torch",
            Key = "torch",
            HitChance = 50,
            Rareness = ItemRareness.Trash,
            Weight = 8,
            MinDamage = new Dictionary<Element, int>
            {
                {Element.Fire, 1},
                {Element.Blunt, 1}
            },
            MaxDamage = new Dictionary<Element, int>
            {
                {Element.Fire, 3},
                {Element.Blunt, 3}
            }
        })
        {
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(InventoryImage);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImage);
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new[]
            {
                new StyledLine {$"Weight: {Weight}"},
                StyledLine.Empty,
                new StyledLine { "Rude torch made from wood and clothes."}, 
            };
        }

        public bool IsLightOn => true;

        public LightLevel LightPower => LightLevel.Dim1;

        public Color LightColor => Color.Orange;

        public ILightSource[] LightSources => new ILightSource[]
        {
            this
        };
    }
}