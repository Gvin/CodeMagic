using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing.ImageProviding;

namespace CodeMagic.UI.Sad.Drawing
{
    public static class CellImageHelper
    {
        private const int DamageTextYShift = 1;
        private static readonly TimeSpan DamageMarksLifeTime = TimeSpan.FromSeconds(2);
        private static readonly SymbolsImage EmptyImage = new SymbolsImage(Program.MapCellImageSize, Program.MapCellImageSize);
        private static readonly ImagesFactory ImagesFactory;
        private static readonly LightLevelManager LightLevelManager;

        static CellImageHelper()
        {
            ImagesFactory = new ImagesFactory(ImagesStorage.Current);
            LightLevelManager = new LightLevelManager();
        }

        public static SymbolsImage GetCellImage(AreaMapCell cell)
        {
            if (cell == null)
                return EmptyImage;

            var image = GetFloorImage(cell);

            var objectsImages = cell.Objects
                .Where(obj => obj.IsVisible)
                .OrderBy(obj => obj.ZIndex)
                .Select(GetObjectImage)
                .Where(img => img != null);

            foreach (var objectImage in objectsImages)
            {
                image = SymbolsImage.Combine(image, objectImage);
            }

            image = LightLevelManager.ApplyLightLevel(image, cell.LightLevel);
            return ApplyDamageMarks(cell, image);
        }

        private static SymbolsImage ApplyDamageMarks(AreaMapCell cell, SymbolsImage image)
        {
            var bigObject = cell.Objects.OfType<IDestroyableObject>().FirstOrDefault(obj => obj.BlocksMovement);
            if (bigObject == null || bigObject.DamageRecords.Length == 0)
                return image;

            var latestRecord = bigObject.DamageRecords
                .OfType<DamageRecord>()
                .Where(rec => rec.CreatedAt + DamageMarksLifeTime > DateTime.Now)
                .OrderByDescending(obj => obj.CreatedAt)
                .FirstOrDefault();
            if (latestRecord == null)
                return image;

            var xnaColor = DamageColorHelper.GetDamageTextColor(latestRecord.Element);
            var color = ColorHelper.ConvertFromXna(xnaColor);
            var damageText = latestRecord.Value.ToString();

            var xShift = 1;
            if (damageText.Length == 3)
            {
                xShift = 0;
            }

            if (damageText.Length > 3)
            {
                damageText = "XXX";
            }

            var damageTextImage = new SymbolsImage(image.Width, image.Height);
            for (int shift = 0; shift < damageText.Length; shift++)
            {
                var x = xShift + shift;
                if (x >= damageTextImage.Width)
                    break;

                damageTextImage.SetPixel(x, DamageTextYShift, damageText[shift], color);
            }

            return SymbolsImage.Combine(image, damageTextImage);
        }

        private static SymbolsImage GetObjectImage(IMapObject mapObject)
        {
            return ImagesFactory.GetImage(mapObject);
        }

        private static SymbolsImage GetFloorImage(AreaMapCell cell)
        {
            return ImagesStorage.Current.GetImage("Floor_Stone");
        }
    }
}