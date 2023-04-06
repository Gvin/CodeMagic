using System;
using System.Collections.Generic;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public abstract class HoldableDurableItemBase : DurableItem, IHoldableItem, IWorldImageProvider, IInventoryImageProvider, IEquippedImageProvider, IDescriptionProvider
    {
        private const string SaveKeyInventoryImage = "InventoryImage";
        private const string SaveKeyWorldImage = "WorldImage";
        private const string SaveKeyEquippedImageRight = "EquippedImageRight";
        private const string SaveKeyEquippedImageLeft = "EquippedImageLeft";

        private readonly SymbolsImage worldImage;
        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage equippedImageRight;
        private readonly SymbolsImage equippedImageLeft;

        protected HoldableDurableItemBase(SaveData data) : base(data)
        {
            inventoryImage = data.GetObject<SymbolsImageSaveable>(SaveKeyInventoryImage)?.GetImage();
            worldImage = data.GetObject<SymbolsImageSaveable>(SaveKeyWorldImage)?.GetImage();
            equippedImageRight = data.GetObject<SymbolsImageSaveable>(SaveKeyEquippedImageRight)?.GetImage();
            equippedImageLeft = data.GetObject<SymbolsImageSaveable>(SaveKeyEquippedImageLeft)?.GetImage();
        }

        protected HoldableDurableItemBase(HoldableItemConfiguration configuration) 
            : base(configuration)
        {
            worldImage = configuration.WorldImage;
            inventoryImage = configuration.InventoryImage;
            equippedImageRight = configuration.EquippedImageRight;
            equippedImageLeft = configuration.EquippedImageLeft;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();

            data.Add(SaveKeyInventoryImage, inventoryImage != null ? new SymbolsImageSaveable(inventoryImage) : null);
            data.Add(SaveKeyWorldImage, worldImage != null ? new SymbolsImageSaveable(worldImage) : null);
            data.Add(SaveKeyEquippedImageRight, equippedImageRight != null ? new SymbolsImageSaveable(equippedImageRight) : null);
            data.Add(SaveKeyEquippedImageLeft, equippedImageLeft != null ? new SymbolsImageSaveable(equippedImageLeft) : null);

            return data;
        }

        public virtual SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return worldImage;
        }

        public virtual SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return inventoryImage;
        }

        public SymbolsImage GetEquippedImage(Player player, IImagesStorage imagesStorage)
        {
            if (Equals(player.Equipment.RightHandItem))
                return GetRightEquippedImage(imagesStorage);
            if (Equals(player.Equipment.LeftHandItem))
                return GetLeftEquippedImage(imagesStorage);
#if DEBUG
            throw new ApplicationException($"Trying to render item \"{Name}\" that not equipped on both left and right hand.");
#else
            return null;
#endif
        }

        protected virtual SymbolsImage GetRightEquippedImage(IImagesStorage storage)
        {
            return equippedImageRight;
        }

        protected virtual SymbolsImage GetLeftEquippedImage(IImagesStorage storage)
        {
            return equippedImageLeft;
        }

        public int EquippedImageOrder => 999;

        public abstract StyledLine[] GetDescription(Player player);
    }

    public class HoldableItemConfiguration : DurableItemConfiguration
    {
        public SymbolsImage WorldImage { get; set; }

        public SymbolsImage InventoryImage { get; set; }

        public SymbolsImage EquippedImageRight { get; set; }

        public SymbolsImage EquippedImageLeft { get; set; }
    }
}