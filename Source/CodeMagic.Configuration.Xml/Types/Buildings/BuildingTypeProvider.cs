using System;
using System.Collections.Generic;
using CodeMagic.Implementations.Objects.Buildings;
using CodeMagic.Implementations.Objects.Buildings.PalisadeBuilding;
using CodeMagic.Implementations.Objects.Buildings.StoneOuterWallBuilding;
using CodeMagic.Implementations.Objects.Buildings.StoneWallBuilding;
using CodeMagic.Implementations.Objects.Buildings.WoodenWallBuilding;

namespace CodeMagic.Configuration.Xml.Types.Buildings
{
    public static class BuildingTypeProvider
    {
        private static readonly Dictionary<string, Type> Buildings = new Dictionary<string, Type>
        {
            {"palisade", typeof(Palisade)},
            {"palisade_gates", typeof(PalisadeGates)},
            {"palisade_embrasure", typeof(PalisadeEmbrasure)},

            {"stone_outer_wall", typeof(StoneOuterWall)},
            {"stone_outer_wall_gates", typeof(StoneOuterWallGates)},
            {"stone_outer_wall_embrasure", typeof(StoneOuterWallEmbrasure)},

            {"box", typeof(Box)},
            {"chest", typeof(Chest)},
            {"furnace", typeof(Furnace)},

            {WoodenWall.BuildingKey, typeof(WoodenWall)},
            {WoodenWallDoor.BuildingKey, typeof(WoodenWallDoor)},
            {WoodenWallGlassWindow.BuildingKey, typeof(WoodenWallGlassWindow)},

            {StoneWall.BuildingKey, typeof(StoneWall)},
            {StoneWallDoor.BuildingKey, typeof(StoneWallDoor)},
            {StoneWallGlassWindow.BuildingKey, typeof(StoneWallGlassWindow)}
        };

        public static Type GetBuildingType(string buildingKey)
        {
            if (Buildings.ContainsKey(buildingKey))
                return Buildings[buildingKey];

            throw new ArgumentException($"Unknown building type: {buildingKey}");
        }
    }
}