using CodeMagic.Core.Area;

namespace CodeMagic.Game.MapGeneration.Dungeon.ObjectsGenerators
{
    internal interface IObjectsGenerator
    {
        void GenerateObjects(IAreaMap map);
    }
}