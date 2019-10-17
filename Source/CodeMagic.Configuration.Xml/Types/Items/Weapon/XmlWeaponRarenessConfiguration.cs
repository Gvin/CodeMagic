﻿using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.Configuration.Xml.Types.Items.Weapon
{
    [Serializable]
    public class XmlWeaponRarenessConfiguration : IWeaponRarenessConfiguration
    {
        [XmlAttribute("value")]
        public ItemRareness Rareness { get; set; }

        [XmlIgnore]
        public IElementConfiguration[] Damage => DamageData.ToArray<IElementConfiguration>();

        [XmlArray("damage")]
        [XmlArrayItem("value")]
        public XmlElementConfiguration[] DamageData { get; set; }

        [XmlElement("min-max-damage-difference")]
        public int MinMaxDamageDifference { get; set; }

        [XmlElement("min-hit-chance")]
        public int MinHitChance { get; set; }

        [XmlElement("max-hit-chance")]
        public int MaxHitChance { get; set; }

        [XmlElement("min-bonuses")]
        public int MinBonuses { get; set; }

        [XmlElement("max-bonuses")]
        public int MaxBonuses { get; set; }

        [XmlArray("materials")]
        [XmlArrayItem("material")]
        public ItemMaterial[] Materials { get; set; }
    }
}