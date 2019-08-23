using CodeMagic.Core.Items;

namespace CodeMagic.ItemsGeneration.Implementations.Usable
{
    public interface IUsableItemTypeGenerator
    {
        IItem Generate(ItemRareness rareness);
    }
}