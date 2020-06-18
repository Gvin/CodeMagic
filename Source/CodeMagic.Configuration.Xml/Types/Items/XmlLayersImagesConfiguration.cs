using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;

namespace CodeMagic.Configuration.Xml.Types.Items
{ 
    [Serializable]
    public class XmlLayersImagesConfiguration : ILayersImagesConfiguration
    {
        [XmlIgnore]
        public ILayersImageSpriteConfiguration[] Sprites => SpritesData?.Cast<ILayersImageSpriteConfiguration>().ToArray();

        [XmlElement("sprite")]
        public XmlLayersImageSpriteConfiguration[] SpritesData { get; set; }
    }

    [Serializable]
    public class XmlLayersImageSpriteConfiguration : ILayersImageSpriteConfiguration
    {
        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlElement("image")]
        public string[] Images { get; set; }
    }
}