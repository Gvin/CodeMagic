using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMagic.Game;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Mono.Drawing;

public class ImagesStorage : IImagesStorage
{
    private const string ImagesFolder = @".\Resources\Images\";
    private const string ImagesExtensionFilter = "*.simg";
    private const string BatchesRegex = "(.*)(?:_\\d*)$";

    private readonly Dictionary<string, SymbolsImage> _images;
    private readonly Dictionary<string, List<SymbolsImage>> _animations;

    public ImagesStorage()
    {
        _images = new Dictionary<string, SymbolsImage>();
        _animations = new Dictionary<string, List<SymbolsImage>>();
    }

    public async Task Load()
    {
        var imageFiles = Directory.GetFiles(ImagesFolder, ImagesExtensionFilter, SearchOption.AllDirectories);
        foreach (var imageFile in imageFiles)
        {
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(imageFile);
            if (string.IsNullOrEmpty(nameWithoutExtension))
                throw new ApplicationException($"Invalid image file name format: {imageFile}");

            var fileName = nameWithoutExtension.ToLower();
            await AddImage(fileName, imageFile);
        }
    }

    private async Task AddImage(string fileName, string filePath)
    {
        var image = await LoadImage(filePath);

        var batchRegex = new Regex(BatchesRegex);
        var match = batchRegex.Match(fileName);

        if (!match.Success)
        {
            _images.Add(fileName, image);
            return;
        }

        var batchName = match.Groups[1].Value.ToLower();
        if (!_animations.ContainsKey(batchName))
        {
            _animations.Add(batchName, new List<SymbolsImage>());
        }
        _animations[batchName].Add(image);
    }

    public SymbolsImage GetImage(string name)
    {
        var unifiedKey = name.ToLower();
        if (_images.ContainsKey(unifiedKey))
            return _images[unifiedKey];

        throw new ApplicationException($"Image not found for key {name}");
    }

    public SymbolsImage[] GetAnimation(string name)
    {
        var unifiedKey = name.ToLower();
        if (_animations.ContainsKey(unifiedKey))
            return _animations[unifiedKey].ToArray();

        throw new ApplicationException($"Animation not found for key {name}");
    }

    private static async Task<SymbolsImage> LoadImage(string filePath)
    {
        await using var fileStream = File.OpenRead(filePath);
        return await SymbolsImage.LoadFromFileAsync(fileStream);
    }
}
