using System;
using System.Xml.Serialization;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Items;

namespace CodeMagic.Configuration.Xml.Types
{
    [Serializable]
    public class XmlLootConfiguration : ILootConfiguration
    {
        [XmlIgnore]
        public IStandardLootConfiguration Weapon => WeaponData;

        [XmlElement("weapon")]
        public XmlStandardLootConfiguration WeaponData { get; set; }

        [XmlIgnore]
        public IArmorLootConfiguration Armor => ArmorData;

        [XmlElement("armor")]
        public XmlArmorLootConfiguration ArmorData { get; set; }

        [XmlIgnore]
        public IStandardLootConfiguration SpellBook => SpellBookData;

        [XmlElement("spell-book")]
        public XmlStandardLootConfiguration SpellBookData { get; set; }

        [XmlIgnore]
        public IStandardLootConfiguration Potion => PotionData;

        [XmlElement("potion")]
        public XmlStandardLootConfiguration PotionData { get; set; }
    }

    [Serializable]
    public class XmlArmorLootConfiguration : XmlStandardLootConfiguration, IArmorLootConfiguration
    {
        [XmlIgnore]
        public IChanceConfiguration<ArmorClass>[] Class => ClassData;

        [XmlArray("class-configuration")]
        [XmlArrayItem("value")]
        public XmlChanceConfiguration<ArmorClass>[] ClassData { get; set; }
    }

    [Serializable]
    public class XmlStandardLootConfiguration : IStandardLootConfiguration
    {
        [XmlIgnore]
        public IChanceConfiguration<int>[] Count => CountData;

        [XmlArray("count-configuration")]
        [XmlArrayItem("value")]
        public XmlChanceConfiguration<int>[] CountData { get; set; }
        
        [XmlIgnore]
        public IChanceConfiguration<ItemRareness>[] Rareness => RarenessData;

        [XmlArray("rareness-configuration")]
        [XmlArrayItem("value")]
        public XmlChanceConfiguration<ItemRareness>[] RarenessData { get; set; }
    }

    [Serializable]
    public class XmlChanceConfiguration<TValue> : IChanceConfiguration<TValue>
    {
        [XmlAttribute("chance")]
        public int Chance { get; set; }

        [XmlText]
        public TValue Value { get; set; }
    }
}