using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Game.MapGeneration.Dungeon.ObjectsGenerators
{
    internal interface IObjectsGenerator
    {
        void GenerateObjects(IAreaMap map, Point playerPosition);
    }
}