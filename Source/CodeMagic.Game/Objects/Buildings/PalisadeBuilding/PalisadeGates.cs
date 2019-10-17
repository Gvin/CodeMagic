using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings.PalisadeBuilding
{
    public class PalisadeGates : DoorObject, IWorldImageProvider
    {
        private const string WorldImageVerticalClosed = "Building_PalisadeGates_Vertical_Closed";
        private const string WorldImageVerticalOpened = "Building_PalisadeGates_Vertical_Opened";
        private const string WorldImageHorizontalClosed = "Building_PalisadeGates_Horizontal_Closed";
        private const string WorldImageHorizontalOpened = "Building_PalisadeGates_Horizontal_Opened";

        public override string Name => "Palisade Gates";

        public override bool BlocksEnvironment => false;

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is Palisade || mapObject is PalisadeEmbrasure || mapObject is PalisadeGates;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
            {
                return Closed
                    ? storage.GetImage(WorldImageVerticalClosed)
                    : storage.GetImage(WorldImageVerticalOpened);
            }

            return Closed
                ? storage.GetImage(WorldImageHorizontalClosed)
                : storage.GetImage(WorldImageHorizontalOpened);
        }
    }
}