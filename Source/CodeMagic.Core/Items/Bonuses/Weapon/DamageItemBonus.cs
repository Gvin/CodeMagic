using CodeMagic.Core.Game;

namespace CodeMagic.Core.Items.Bonuses.Weapon
{
    public class DamageItemBonus : IWeaponItemBonus
    {
        public DamageItemBonus(int maxDamage, int minDamage, Element element)
        {
            MaxDamage = maxDamage;
            MinDamage = minDamage;
            Element = element;
        }

        public int MaxDamage { get; }
        public int MinDamage { get; }
        public Element Element { get; }
    }
}