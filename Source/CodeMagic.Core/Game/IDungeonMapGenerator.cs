using CodeMagic.Core.Area;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Game
{
    public interface IDungeonMapGenerator : IInjectable
    {
        IAreaMap GenerateNewMap(int level, ItemRareness rareness, int maxLevel, out Point playerPosition);
    }
}