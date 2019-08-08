using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Items
{
    public class ArmorItem : EquipableItem
    {
        private readonly Dictionary<Element, int> protection;

        public ArmorItem(ArmorItemConfiguration configuration) 
            : base(configuration)
        {
            protection = configuration.Protection.ToDictionary(pair => pair.Key, pair => pair.Value);
            ArmorType = configuration.ArmorType;
        }

        public ArmorType ArmorType { get; }

        public int GetProtection(Element element)
        {
            return protection.ContainsKey(element) ? protection[element] : 0;
        }

        public override bool Stackable => false;
    }

    public enum ArmorType
    {
        Helmet = 0,
        Chest = 1,
        Leggings = 2
    }

    public class ArmorItemConfiguration : EquipableItemConfiguration
    {
        public ArmorItemConfiguration()
        {
            Protection = new Dictionary<Element, int>();
        }

        public Dictionary<Element, int> Protection { get; set; }

        public ArmorType ArmorType { get; set; }
    }
}