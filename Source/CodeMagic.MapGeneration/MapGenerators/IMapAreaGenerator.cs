using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.MapGeneration.MapGenerators
{
    internal interface IMapAreaGenerator
    {
        IAreaMap Generate(MapSize size, out Point playerPosition);
    }
}