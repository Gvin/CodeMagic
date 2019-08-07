namespace CodeMagic.Core.Items.Bonuses.Equipable
{
    public interface IEquipableItemBonus : IItemBonus
    {
        int MaxHealth { get; }

        int MaxMana { get; }

        int ManaRegeneration { get; }
    }
}