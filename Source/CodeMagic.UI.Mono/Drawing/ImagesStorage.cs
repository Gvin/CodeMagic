using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CodeMagic.Game;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Mono.Drawing
{
    public class ImagesStorage : IImagesStorage
    {
        private const string ImagesFolder = @".\Resources\Images\";
        private const string ImagesExtensionFilter = "*.simg";
        private const string BatchesRegex = "(.*)(?:_\\d*)$";

        public static ImagesStorage Current { get; } = new ImagesStorage();

        private readonly Dictionary<string, SymbolsImage> images;
        private readonly Dictionary<string, List<SymbolsImage>> animations;

        private ImagesStorage()
        {
            images = new Dictionary<string, SymbolsImage>();
            animations = new Dictionary<string, List<SymbolsImage>>();
        }

        public void Load()
        {
            var imageFiles = Directory.GetFiles(ImagesFolder, ImagesExtensionFilter, SearchOption.AllDirectories);
            foreach (var imageFile in imageFiles)
            {
                var nameWithoutExtension = Path.GetFileNameWithoutExtension(imageFile);
                if (string.IsNullOrEmpty(nameWithoutExtension))
                    throw new ApplicationException($"Invalid image file name format: {imageFile}");

                var fileName = nameWithoutExtension.ToLower();
                AddImage(fileName, imageFile);
            }
        }

        private void AddImage(string fileName, string filePath)
        {
            var image = LoadImage(filePath);

            var batchRegex = new Regex(BatchesRegex);
            var match = batchRegex.Match(fileName);

            if (!match.Success)
            {
                images.Add(fileName, image);
                return;
            }

            var batchName = match.Groups[1].Value.ToLower();
            if (!animations.ContainsKey(batchName))
            {
                animations.Add(batchName, new List<SymbolsImage>());
            }
            animations[batchName].Add(image);
        }

        private SymbolsImage LoadImage(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                return SymbolsImage.LoadFromFile(fileStream);
            }
        }

        public SymbolsImage GetImage(string name)
        {
            var unifiedKey = name.ToLower();
            if (images.ContainsKey(unifiedKey))
                return images[unifiedKey];

            throw new ApplicationException($"Image not found for key {name}");
        }

        public SymbolsImage[] GetAnimation(string name)
        {
            var unifiedKey = name.ToLower();
            if (animations.ContainsKey(unifiedKey))
                return animations[unifiedKey].ToArray();

            throw new ApplicationException($"Animation not found for key {name}");
        }
    }
}