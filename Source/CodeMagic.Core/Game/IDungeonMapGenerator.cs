using CodeMagic.Core.Area;
using CodeMagic.Core.Injection;

namespace CodeMagic.Core.Game
{
    public interface IDungeonMapGenerator : IInjectable
    {
        IAreaMap GenerateNewMap(int level, out Point playerPosition);
    }
}