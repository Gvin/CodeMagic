using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;
using CodeMagic.Implementations;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Sad.Drawing.ImageProviding
{
    public class WorldImagesFactory
    {
        private const string ImageStatusOnFire = "Status_OnFire";
        private const string ImageStatusOily = "Status_Oily";
        private const string ImageStatusWet = "Status_Wet";
        private const string ImageStatusBlind = "Status_Blind";
        private const string ImageStatusParalyzed = "Status_Paralyzed";
        private const string ImageStatusFrozen = "Status_Frozen";

        private readonly IImagesStorage imagesStorage;

        public WorldImagesFactory(IImagesStorage imagesStorage)
        {
            this.imagesStorage = imagesStorage;
        }

        public SymbolsImage GetImage(object objectToDraw)
        {
            var objectImage = GetObjectImage(objectToDraw);
            if (objectImage == null)
                return null;

            if (objectToDraw is IDestroyableObject destroyable)
            {
                objectImage = ApplyDestroyableStatuses(destroyable, objectImage);
            }

            return objectImage;
        }

        private SymbolsImage ApplyDestroyableStatuses(IDestroyableObject destroyable, SymbolsImage image)
        {
            if (destroyable.Statuses.Contains(OnFireObjectStatus.StatusType))
            {
                return ApplyStatusImage(image, ImageStatusOnFire);
            }

            if (destroyable.Statuses.Contains(ParalyzedObjectStatus.StatusType))
            {
                return ApplyStatusImage(image, ImageStatusParalyzed);
            }

            if (destroyable.Statuses.Contains(FrozenObjectStatus.StatusType))
            {
                return ApplyStatusImage(image, ImageStatusFrozen);
            }

            if (destroyable.Statuses.Contains(BlindObjectStatus.StatusType))
            {
                return ApplyStatusImage(image, ImageStatusBlind);
            }

            if (destroyable.Statuses.Contains(OilyObjectStatus.StatusType))
            {
                return ApplyStatusImage(image, ImageStatusOily);
            }

            if (destroyable.Statuses.Contains(WetObjectStatus.StatusType))
            {
                return ApplyStatusImage(image, ImageStatusWet);
            }

            return image;
        }

        private SymbolsImage ApplyStatusImage(SymbolsImage initialImage, string statusImageName)
        {
            var statusImage = imagesStorage.GetImage(statusImageName);
            return SymbolsImage.Combine(initialImage, statusImage);
        }

        private SymbolsImage GetObjectImage(object objectToDraw)
        {
            if (objectToDraw is IWorldImageProvider selfProvider)
            {
                return selfProvider.GetWorldImage(imagesStorage);
            }

            return null;
        }
    }
}