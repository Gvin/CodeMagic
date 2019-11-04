using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings.WoodenWallBuilding
{
    public class WoodenWallGlassWindow : DoorBase, IWorldImageProvider, IRoofSupport
    {
        public const string BuildingKey = "wooden_wall_glass_window";

        public override string Name => "Glass Window";

        public override bool BlocksVisibility => false;

        public override bool BlocksMovement => true;

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is WoodenWall || mapObject is WoodenWallDoor || mapObject is WoodenWallGlassWindow;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
            {
                return Closed
                    ? storage.GetImage("Building_WoodenWallGlassWindow_Vertical_Closed")
                    : storage.GetImage("Building_WoodenWallGlassWindow_Vertical_Opened");
            }

            return Closed
                ? storage.GetImage("Building_WoodenWallGlassWindow_Horizontal_Closed")
                : storage.GetImage("Building_WoodenWallGlassWindow_Horizontal_Opened");
        }
    }
}