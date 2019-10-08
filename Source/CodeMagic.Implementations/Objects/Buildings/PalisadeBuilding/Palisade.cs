using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Buildings.PalisadeBuilding
{
    public class Palisade : WallBase, IWorldImageProvider
    {
        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (HasConnectedTile(1, 0) && HasConnectedTile(-1, 0) && 
                HasConnectedTile(0, 1) && HasConnectedTile(0, -1))
                return storage.GetImage("Building_Palisade_Top_Bottom_Left_Right");

            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1) &&
                HasConnectedTile(1, 0))
                return storage.GetImage("Building_Palisade_Top_Bottom_Right");
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1) &&
                HasConnectedTile(-1, 0))
                return storage.GetImage("Building_Palisade_Top_Bottom_Left");
            if (HasConnectedTile(1, 0) && HasConnectedTile(-1, 0) && 
                HasConnectedTile(0, 1))
                return storage.GetImage("Building_Palisade_Bottom_Left_Right");
            if (HasConnectedTile(1, 0) && HasConnectedTile(-1, 0) && 
                HasConnectedTile(0, -1))
                return storage.GetImage("Building_Palisade_Top_Left_Right");

            if (HasConnectedTile(0, -1) && HasConnectedTile(1, 0))
                return storage.GetImage("Building_Palisade_Top_Right");
            if (HasConnectedTile(0, 1) && HasConnectedTile(1, 0))
                return storage.GetImage("Building_Palisade_Bottom_Right");
            if (HasConnectedTile(0, -1) && HasConnectedTile(-1, 0))
                return storage.GetImage("Building_Palisade_Top_Left");
            if (HasConnectedTile(0, 1) && HasConnectedTile(-1, 0))
                return storage.GetImage("Building_Palisade_Bottom_Left");

            if (HasConnectedTile(0, 1) && HasConnectedTile(0, -1))
                return storage.GetImage("Building_Palisade_Vertical");
            if (HasConnectedTile(0, 1) && !HasConnectedTile(0, -1))
                return storage.GetImage("Building_Palisade_Vertical_TopEnd");
            if (!HasConnectedTile(0, 1) && HasConnectedTile(0, -1))
                return storage.GetImage("Building_Palisade_Vertical_BottomEnd");

            return storage.GetImage("Building_Palisade_Horizontal");
        }

        public override string Name => "Palisade";

        public override bool BlocksEnvironment => false;

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is Palisade || mapObject is PalisadeEmbrasure || mapObject is PalisadeGates;
        }
    }
}