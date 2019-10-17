using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Usable
{
    public interface IUsableItemTypeGenerator
    {
        IItem Generate(ItemRareness rareness);
    }
}