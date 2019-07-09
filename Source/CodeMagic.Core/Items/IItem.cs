namespace CodeMagic.Core.Items
{
    public interface IItem
    {
        string Name { get; }

        string Key { get; }

        ItemRareness Rareness { get; }

        int Weight { get; }

        bool Stackable { get; }
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