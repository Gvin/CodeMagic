using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon
{ 
    [Serializable]
    public class XmlWeaponImagesConfiguration : IWeaponImagesConfiguration
    {
        [XmlIgnore]
        public IWeaponImageSpriteConfiguration[] Sprites => SpritesData?.Cast<IWeaponImageSpriteConfiguration>().ToArray();

        [XmlElement("sprite")]
        public XmlWeaponImageSpriteConfiguration[] SpritesData { get; set; }
    }

    [Serializable]
    public class XmlWeaponImageSpriteConfiguration : IWeaponImageSpriteConfiguration
    {
        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlElement("image")]
        public string[] Images { get; set; }
    }
}