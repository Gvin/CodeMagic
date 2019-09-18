using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.MapGeneration.Dungeon.MapGenerators
{
    internal interface IMapAreaGenerator
    {
        IAreaMap Generate(MapSize size, out Point playerPosition);
    }
}