using CodeMagic.Core.Items;

namespace CodeMagic.Core.Objects.Creatures.Loot
{
    public interface ILootGenerator
    {
        IItem[] GenerateLoot();
    }
}