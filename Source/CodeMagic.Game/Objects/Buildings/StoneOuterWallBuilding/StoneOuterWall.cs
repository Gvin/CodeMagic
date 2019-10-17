using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings.StoneOuterWallBuilding
{
    public class StoneOuterWall : WallBase, IWorldImageProvider
    {
        public override string Name => "Stone Wall";

        public override bool BlocksEnvironment => false;

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is StoneOuterWall || mapObject is StoneOuterWallEmbrasure || mapObject is StoneOuterWallGates;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (HasConnectedTile(1, 0) && HasConnectedTile(-1, 0) &&
                HasConnectedTile(0, 1) && HasConnectedTile(0, -1))
                return storage.GetImage("Building_StoneOuterWall_Top_Bottom_Left_Right");

            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1) &&
                HasConnectedTile(1, 0))
                return storage.GetImage("Building_StoneOuterWall_Top_Bottom_Right");
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1) &&
                HasConnectedTile(-1, 0))
                return storage.GetImage("Building_StoneOuterWall_Top_Bottom_Left");
            if (HasConnectedTile(1, 0) && HasConnectedTile(-1, 0) &&
                HasConnectedTile(0, 1))
                return storage.GetImage("Building_StoneOuterWall_Bottom_Left_Right");
            if (HasConnectedTile(1, 0) && HasConnectedTile(-1, 0) &&
                HasConnectedTile(0, -1))
                return storage.GetImage("Building_StoneOuterWall_Top_Left_Right");

            if (HasConnectedTile(0, -1) && HasConnectedTile(1, 0))
                return storage.GetImage("Building_StoneOuterWall_Top_Right");
            if (HasConnectedTile(0, 1) && HasConnectedTile(1, 0))
                return storage.GetImage("Building_StoneOuterWall_Bottom_Right");
            if (HasConnectedTile(0, -1) && HasConnectedTile(-1, 0))
                return storage.GetImage("Building_StoneOuterWall_Top_Left");
            if (HasConnectedTile(0, 1) && HasConnectedTile(-1, 0))
                return storage.GetImage("Building_StoneOuterWall_Bottom_Left");

            if (HasConnectedTile(0, 1) && HasConnectedTile(0, -1))
                return storage.GetImage("Building_StoneOuterWall_Vertical");
            if (HasConnectedTile(0, 1) && !HasConnectedTile(0, -1))
                return storage.GetImage("Building_StoneOuterWall_Vertical_TopEnd");
            if (!HasConnectedTile(0, 1) && HasConnectedTile(0, -1))
                return storage.GetImage("Building_StoneOuterWall_Vertical_BottomEnd");

            return storage.GetImage("Building_StoneOuterWall_Horizontal");
        }
    }
}