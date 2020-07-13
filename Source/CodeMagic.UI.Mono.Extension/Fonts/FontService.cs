using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace CodeMagic.UI.Mono.Extension.Fonts
{
    public class FontService
    {
        public IFont LoadFont(GraphicsDevice graphicsDevice, string fontConfigFilePath)
        {
            if (!File.Exists(fontConfigFilePath))
                throw new FileNotFoundException($"Font config file not found: {fontConfigFilePath}");

            try
            {
                var content = File.ReadAllText(fontConfigFilePath);
                var fontConfig = (FontConfig) JsonConvert.DeserializeObject(content, typeof(FontConfig));
                var configFolder = Path.GetDirectoryName(fontConfigFilePath);
                var fontTextureFilePath = Path.Combine(configFolder, fontConfig.FilePath);

                if (!File.Exists(fontTextureFilePath))
                    throw new FileNotFoundException($"Font texture file not found: {fontTextureFilePath}");

                var fontTexture = Texture2D.FromFile(graphicsDevice, fontTextureFilePath);
                return new Font(fontTexture, fontConfig.GlyphWidth, fontConfig.GlyphHeight, fontConfig.Columns, fontConfig.GlyphPadding, fontConfig.SolidGlyphIndex);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Unable to load font from config path {fontConfigFilePath}", exception);
            }
        }

        public class FontConfig
        {
            public string FilePath { get; set; }

            public int GlyphWidth { get; set; }

            public int GlyphHeight { get; set; }

            public int GlyphPadding { get; set; }

            public int SolidGlyphIndex { get; set; }

            public int Columns { get; set; }
        }
    }
}