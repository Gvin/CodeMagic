using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items.Bonuses.Armor;

namespace CodeMagic.Core.Items
{
    public class ArmorItem : Item, IEquipableItem
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
            var bonuses = Bonuses.OfType<IArmorItemBonus>().ToDictionary(bonus => bonus.Element, bonus => bonus.Protection);
            var bonusValue = bonuses.ContainsKey(element) ? bonuses[element] : 0;
            var defaultValue = protection.ContainsKey(element) ? protection[element] : 0;

            return bonusValue + defaultValue;
        }

        public override bool Stackable => false;
    }

    public enum ArmorType
    {
        Helmet = 0,
        Chest = 1,
        Leggings = 2
    }

    public class ArmorItemConfiguration : ItemConfiguration
    {
        public ArmorItemConfiguration()
        {
            Protection = new Dictionary<Element, int>();
        }

        public Dictionary<Element, int> Protection { get; set; }

        public ArmorType ArmorType { get; set; }
    }
}