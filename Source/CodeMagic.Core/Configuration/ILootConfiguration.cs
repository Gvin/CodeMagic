using CodeMagic.Core.Items;

namespace CodeMagic.Core.Configuration
{
    public interface ILootConfiguration
    {
        IStandardLootConfiguration Weapon { get; }

        IArmorLootConfiguration Armor { get; }

        IStandardLootConfiguration SpellBook { get; }

        IStandardLootConfiguration Potion { get; }
    }

    public interface IArmorLootConfiguration : IStandardLootConfiguration
    {
        IChanceConfiguration<ArmorClass>[] Class { get; }
    }

    public interface IStandardLootConfiguration
    {
        IChanceConfiguration<int>[] Count { get; }

        IChanceConfiguration<ItemRareness>[] Rareness { get; }
    }

    public interface IChanceConfiguration<TValue>
    {
        int Chance { get; }

        TValue Value { get; }
    }
}