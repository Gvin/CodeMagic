using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Game.MapGeneration.Dungeon.MonstersGenerators
{
    internal interface IMonstersGenerator
    {
        void GenerateMonsters(IAreaMap map, Point playerPosition);
    }
}