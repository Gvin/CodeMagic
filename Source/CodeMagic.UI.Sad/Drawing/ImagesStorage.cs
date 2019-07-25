using System;
using System.Collections.Generic;
using System.IO;
using CodeMagic.Objects.Implementation;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Sad.Drawing
{
    public class ImagesStorage : IImagesStorage
    {
        private const string ImagesFolder = @".\Resources\Images\";
        private const string ImagesExtensionFilter = "*.simg";

        public static ImagesStorage Current { get; } = new ImagesStorage();

        private readonly Dictionary<string, SymbolsImage> images;

        private ImagesStorage()
        {
            images = new Dictionary<string, SymbolsImage>();
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
                using (var fileStream = File.OpenRead(imageFile))
                {
                    var image = SymbolsImage.LoadFromFile(fileStream);
                    images.Add(fileName, image);
                }
            }
        }

        public SymbolsImage GetImage(string name)
        {
            var unifiedKey = name.ToLower();
            if (images.ContainsKey(unifiedKey))
                return images[unifiedKey];

            throw new ApplicationException($"Image not found for key {name}");
        }
    }
}