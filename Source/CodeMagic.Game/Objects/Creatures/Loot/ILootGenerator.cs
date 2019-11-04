using CodeMagic.Core.Items;

namespace CodeMagic.Game.Objects.Creatures.Loot
{
    public interface ILootGenerator
    {
        IItem[] GenerateLoot();
    }
}