using CodeMagic.Core.Items;
using CodeMagic.Game.Items;

namespace CodeMagic.Game.Configuration
{
    public interface ILootConfiguration
    {
        IStandardLootConfiguration Weapon { get; }

        IArmorLootConfiguration Armor { get; }

        IStandardLootConfiguration SpellBook { get; }

        IStandardLootConfiguration Usable { get; }

        ISimpleLootConfiguration Resource { get; }
    }

    public interface IArmorLootConfiguration : IStandardLootConfiguration
    {
        IChanceConfiguration<ArmorClass>[] Class { get; }
    }

    public interface IStandardLootConfiguration : ISimpleLootConfiguration
    {
        IChanceConfiguration<ItemRareness>[] Rareness { get; }
    }

    public interface ISimpleLootConfiguration
    {
        IChanceConfiguration<int>[] Count { get; }
    }

    public interface IChanceConfiguration<TValue>
    {
        int Chance { get; }

        TValue Value { get; }
    }
}