﻿using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Implementations.Objects.Buildings.WoodenWallBuilding;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Buildings.StoneWallBuilding
{
    public class StoneWallGlassWindow : DoorObject, IWorldImageProvider
    {
        public const string BuildingKey = "stone_wall_glass_window";

        public override string Name => "Glass Window";

        public override bool BlocksVisibility => false;

        public override bool BlocksMovement => true;

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is StoneWall || mapObject is StoneWallDoor || mapObject is StoneWallGlassWindow;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
            {
                return Closed
                    ? storage.GetImage("Building_StoneWallGlassWindow_Vertical_Closed")
                    : storage.GetImage("Building_StoneWallGlassWindow_Vertical_Opened");
            }

            return Closed
                ? storage.GetImage("Building_StoneWallGlassWindow_Horizontal_Closed")
                : storage.GetImage("Building_StoneWallGlassWindow_Horizontal_Opened");
        }
    }
}