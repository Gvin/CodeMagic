using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings.PalisadeBuilding
{
    public class PalisadeEmbrasure : WallBase, IWorldImageProvider
    {
        private const string WorldImageHorizontal = "Building_PalisadeEmbrasure_Horizontal";
        private const string WorldImageVertical = "Building_PalisadeEmbrasure_Vertical";

        public override string Name => "Palisade Embrasure";

        public override bool BlocksProjectiles => false;

        public override bool BlocksVisibility => false;

        public override bool BlocksEnvironment => false;

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is Palisade || mapObject is PalisadeEmbrasure || mapObject is PalisadeGates;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
                return storage.GetImage(WorldImageVertical);

            return storage.GetImage(WorldImageHorizontal);
        }
    }
}