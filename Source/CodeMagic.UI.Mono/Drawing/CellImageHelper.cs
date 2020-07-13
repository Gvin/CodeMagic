using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.ObjectEffects;
using CodeMagic.UI.Images;
using CodeMagic.UI.Mono.Drawing.ImageProviding;

namespace CodeMagic.UI.Mono.Drawing
{
    public static class CellImageHelper
    {
        private static readonly TimeSpan DamageMarksLifeTime = TimeSpan.FromSeconds(2);
        private static readonly SymbolsImage EmptyImage = new SymbolsImage(Program.MapCellImageSize, Program.MapCellImageSize);
        private static readonly WorldImagesFactory WorldImagesFactory;
        private static readonly LightLevelManager LightLevelManager;

        static CellImageHelper()
        {
            WorldImagesFactory = new WorldImagesFactory(ImagesStorage.Current);
            LightLevelManager = new LightLevelManager(Settings.Current.Brightness);
        }

        public static SymbolsImage GetCellImage(IAreaMapCell cell)
        {
            if (cell == null)
                return EmptyImage;

            var objectsImages = cell.Objects
                .Where(obj => obj.IsVisible)
                .OrderBy(obj => obj.ZIndex)
                .Select(GetObjectImage)
                .Where(img => img != null)
                .ToArray();

            var image = objectsImages.FirstOrDefault();
            if (image == null)
                return EmptyImage;

            foreach (var objectImage in objectsImages.Skip(1))
            {
                image = CombineImages(image, objectImage);
            }

            image = LightLevelManager.ApplyLightLevel(image, cell.LightLevel);
            return ApplyObjectEffects(cell, image);
        }

        private static SymbolsImage CombineImages(SymbolsImage bottom, SymbolsImage top)
        {
            return SymbolsImage.Combine(bottom, top);
        }

        private static SymbolsImage ApplyObjectEffects(IAreaMapCell cell, SymbolsImage image)
        {
            var bigObject = cell.Objects.OfType<IDestroyableObject>().FirstOrDefault(obj => obj.BlocksMovement);
            if (bigObject == null || !bigObject.ObjectEffects.Any())
                return image;

            var latestEffect = bigObject.ObjectEffects
                .OfType<ObjectEffect>()
                .Where(rec => rec.CreatedAt + DamageMarksLifeTime > DateTime.Now)
                .OrderByDescending(obj => obj.CreatedAt)
                .FirstOrDefault();
            if (latestEffect == null)
                return image;

            var effectImage = latestEffect.GetEffectImage(image.Width, image.Height, ImagesStorage.Current);

            return SymbolsImage.Combine(image, effectImage);
        }

        private static SymbolsImage GetObjectImage(IMapObject mapObject)
        {
            return WorldImagesFactory.GetImage(mapObject);
        }
    }
}