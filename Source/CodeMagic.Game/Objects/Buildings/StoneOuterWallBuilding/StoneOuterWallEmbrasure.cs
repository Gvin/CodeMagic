using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings.StoneOuterWallBuilding
{
    public class StoneOuterWallEmbrasure : WallBase, IWorldImageProvider
    {
        private const string WorldImageHorizontal = "Building_StoneOuterWallEmbrasure_Horizontal";
        private const string WorldImageVertical = "Building_StoneOuterWallEmbrasure_Vertical";

        public override string Name => "Stone Wall Embrasure";

        public override bool BlocksProjectiles => false;

        public override bool BlocksVisibility => false;

        public override bool BlocksEnvironment => false;

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is StoneOuterWall || mapObject is StoneOuterWallEmbrasure || mapObject is StoneOuterWallGates;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
                return storage.GetImage(WorldImageVertical);

            return storage.GetImage(WorldImageHorizontal);
        }
    }
}