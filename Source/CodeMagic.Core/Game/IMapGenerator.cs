using CodeMagic.Core.Area;

namespace CodeMagic.Core.Game
{
    public interface IMapGenerator
    {
        IAreaMap GenerateNewMap(int level, out Point playerPosition);
    }
}