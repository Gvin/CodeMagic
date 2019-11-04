using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings.StoneOuterWallBuilding
{
    public class StoneOuterWallGates : DoorBase, IWorldImageProvider
    {
        private const string WorldImageVerticalClosed = "Building_StoneOuterWallGates_Vertical_Closed";
        private const string WorldImageVerticalOpened = "Building_StoneOuterWallGates_Vertical_Opened";
        private const string WorldImageHorizontalClosed = "Building_StoneOuterWallGates_Horizontal_Closed";
        private const string WorldImageHorizontalOpened = "Building_StoneOuterWallGates_Horizontal_Opened";

        public override string Name => "Stone Wall Gates";

        public override bool BlocksEnvironment => false;

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is StoneOuterWall || mapObject is StoneOuterWallEmbrasure || mapObject is StoneOuterWallGates;
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