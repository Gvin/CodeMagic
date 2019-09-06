using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using CodeMagic.UI.Images.Saving.Xml.Types;

namespace CodeMagic.UI.Images.Saving.Xml
{
    internal class XmlSavingManager : ISavingManager
    {
        public void SaveToFile(int width, int height, SymbolsImage.Pixel[][] pixels, Stream fireStream)
        {
            var type = new XmlImageType(width, height, pixels);
            var serializer = new XmlSerializer(typeof(XmlImageType));
            serializer.Serialize(fireStream, type);
        }

        public SymbolsImage LoadFromFile(Stream fileStream)
        {
            var serializer = new XmlSerializer(typeof(XmlImageType));
            var data = serializer.Deserialize(fileStream) as XmlImageType;
            if (data == null)
                throw new SerializationException("Unable to read image.");

            var image = new SymbolsImage(data.Width, data.Height);
            for (var y = 0; y < data.Height; y++)
            {
                for (var x = 0; x < data.Width; x++)
                {
                    var pixel = image[x, y];
                    var dataPixel = data.Rows[y].Pixels[x];

                    pixel.Symbol = dataPixel.Glyph;
                    pixel.Color = dataPixel.Color?.GetColor();
                    pixel.BackgroundColor = dataPixel.BackgroundColor?.GetColor();
                }
            }

            return image;
        }
    }
}