using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class CaveWall : WallBase, IWorldImageProvider, IUsableObject
    {
        private const string ImageNormal = "Wall_Cave";
        private const string ImageBottom = "Wall_Cave_Bottom";
        private const string ImageRight = "Wall_Cave_Right";
        private const string ImageBottomRight = "Wall_Cave_Bottom_Right";
        private const string ImageCorner = "Wall_Cave_Corner";

        private readonly bool destroyable;

        public CaveWall(bool destroyable)
        {
            this.destroyable = destroyable;
        }

        public override string Name => "Cave Wall";

        protected override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is CaveWall;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (!HasConnectedTile(0, 1) && !HasConnectedTile(1, 0))
            {
                return storage.GetImage(ImageBottomRight);
            }

            if (!HasConnectedTile(0, 1))
            {
                return storage.GetImage(ImageBottom);
            }

            if (!HasConnectedTile(1, 0))
            {
                return storage.GetImage(ImageRight);
            }

            if (!HasConnectedTile(1, 1))
            {
                return storage.GetImage(ImageCorner);
            }

            return storage.GetImage(ImageNormal);
        }

        public void Use(IGameCore game, Point position)
        {
            if (!destroyable)
                return;

            game.Journal.Write(new ToolRequiredMessage());
        }
    }
}