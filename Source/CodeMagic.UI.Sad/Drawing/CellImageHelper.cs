using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Drawing.ImageProviding;

namespace CodeMagic.UI.Sad.Drawing
{
    public static class CellImageHelper
    {
        private static readonly SymbolsImage EmptyImage = new SymbolsImage(Program.MapCellImageSize, Program.MapCellImageSize);
        private static readonly ImagesFactory ImagesFactory;

        static CellImageHelper()
        {
            ImagesFactory = new ImagesFactory(ImagesStorage.Current);
        }

        public static SymbolsImage GetCellImage(AreaMapCell cell)
        {
            if (cell == null)
                return EmptyImage;

            var floorImage = GetFloorImage(cell);

            var objectsImages = cell.Objects
                .Where(obj => obj.IsVisible)
                .OrderBy(obj => obj.ZIndex)
                .Select(GetObjectImage)
                .Where(img => img != null);

            foreach (var objectImage in objectsImages)
            {
                floorImage = SymbolsImage.Combine(floorImage, objectImage);
            }

            return floorImage;
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