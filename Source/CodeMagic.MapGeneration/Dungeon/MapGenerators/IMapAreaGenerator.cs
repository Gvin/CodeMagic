using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;

namespace CodeMagic.MapGeneration.Dungeon.MapGenerators
{
    internal interface IMapAreaGenerator
    {
        IAreaMap Generate(MapSize size, ItemRareness rareness, bool isLastLevel, out Point playerPosition);
    }
}