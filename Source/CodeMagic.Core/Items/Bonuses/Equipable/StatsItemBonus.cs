namespace CodeMagic.Core.Items.Bonuses.Equipable
{
    public class StatsItemBonus : IEquipableItemBonus
    {
        public StatsItemBonus(int maxHealth, int maxMana, int manaRegeneration)
        {
            MaxHealth = maxHealth;
            MaxMana = maxMana;
            ManaRegeneration = manaRegeneration;
        }

        public int MaxHealth { get; }
        public int MaxMana { get; }
        public int ManaRegeneration { get; }
    }
}