using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Items
{
    public interface IItem : IMapObject
    {
        string Id { get; }

        string Key { get; }

        ItemRareness Rareness { get; }

        int Weight { get; }

        bool Stackable { get; }

        bool Equals(IItem other);
    }

    public enum ItemRareness
    {
        Trash = 0,
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4
    }
}