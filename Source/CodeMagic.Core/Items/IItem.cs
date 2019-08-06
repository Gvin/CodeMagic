namespace CodeMagic.Core.Items
{
    public interface IItem
    {
        string Id { get; }

        string Name { get; }

        string Key { get; }

        ItemRareness Rareness { get; }

        int Weight { get; }

        bool Stackable { get; }

        bool Equals(IItem other);
    }

    public enum ItemRareness
    {
        Trash,
        Common,
        Uncommon,
        Rare,
        Epic
    }
}