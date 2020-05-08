using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Materials;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations
{
    public class ResourceItemsGenerator
    {
        public IItem GenerateResourceItem(ItemRareness rareness)
        {
            if (rareness == ItemRareness.Common)
                return new BlankScroll();

            return null;
        }
    }
}