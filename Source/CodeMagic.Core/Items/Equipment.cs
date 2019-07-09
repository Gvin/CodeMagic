using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Core.Items
{
    public class Equipment
    {
        private const int HandMinDamage = 0;
        private const int HandMaxDamage = 2;

        public Equipment()
        {
            Armor = new Dictionary<ArmorType, ArmorItem>
            {
                {ArmorType.Arms, null},
                {ArmorType.Head, null},
                {ArmorType.Chest, null},
                {ArmorType.Legs, null}
            };
        }

        public Dictionary<ArmorType, ArmorItem> Armor { get; }

        public WeaponItem Weapon { get; set; }

        public SpellBook SpellBook { get; set; }

        public int MinDamage => Weapon?.DamageMin ?? HandMinDamage;

        public int MaxDamage => Weapon?.DamageMax ?? HandMaxDamage;

        public int Protection
        {
            get
            {
                return Armor.Select(pair => pair.Value).Where(armor => armor != null).Sum(armor => armor.Protection);
            }
        }
    }
}