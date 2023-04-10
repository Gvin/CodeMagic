using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.ObjectEffects;
using CodeMagic.UI.Images;
using CodeMagic.UI.Mono.Drawing.ImageProviding;
using System.Linq;
using CodeMagic.Game;

namespace CodeMagic.UI.Mono.Drawing;

public interface ICellImageService
{
    SymbolsImage GetCellImage(IAreaMapCell cell);
}

public class CellImageService : ICellImageService
{
    private static readonly TimeSpan DamageMarksLifeTime = TimeSpan.FromSeconds(2);
    private static readonly SymbolsImage EmptyImage = new (Program.MapCellImageSize, Program.MapCellImageSize);

    private readonly IWorldImagesFactory _worldImagesFactory;
    private readonly ILightLevelManager _lightLevelManager;
    private readonly IImagesStorage _imagesStorage;

    public CellImageService(IWorldImagesFactory worldImagesFactory, ILightLevelManager lightLevelManager, IImagesStorage imagesStorage)
    {
        _worldImagesFactory = worldImagesFactory;
        _lightLevelManager = lightLevelManager;
        _imagesStorage = imagesStorage;
    }

    public SymbolsImage GetCellImage(IAreaMapCell cell)
    {
        if (cell == null)
        {
            return EmptyImage;
        }

        var objectsImages = cell.Objects
            .Where(obj => obj.IsVisible)
            .OrderBy(obj => obj.ZIndex)
            .Select(GetObjectImage)
            .Where(img => img != null)
            .ToArray();

        var image = objectsImages.FirstOrDefault();
        if (image == null) // No images
        {
            return EmptyImage;
        }

        foreach (var objectImage in objectsImages.Skip(1))
        {
            image = CombineImages(image, objectImage);
        }

        image = _lightLevelManager.ApplyLightLevel(image, cell.LightLevel);
        return ApplyObjectEffects(cell, image);
    }

    private SymbolsImage GetObjectImage(IMapObject mapObject)
    {
        return _worldImagesFactory.GetImage(mapObject);
    }

    private static SymbolsImage CombineImages(SymbolsImage bottom, SymbolsImage top)
    {
        return SymbolsImage.Combine(bottom, top);
    }

    private SymbolsImage ApplyObjectEffects(IAreaMapCell cell, SymbolsImage image)
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

        var effectImage = latestEffect.GetEffectImage(image.Width, image.Height, _imagesStorage);

        return SymbolsImage.Combine(image, effectImage);
    }
}
