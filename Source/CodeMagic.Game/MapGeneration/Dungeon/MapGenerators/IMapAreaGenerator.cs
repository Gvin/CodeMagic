using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapGenerators
{
    internal interface IMapAreaGenerator
    {
        IAreaMap Generate(int level, MapSize size, out Point playerPosition);
    }
}