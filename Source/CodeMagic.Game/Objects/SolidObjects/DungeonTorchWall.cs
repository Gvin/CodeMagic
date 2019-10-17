using System;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class DungeonTorchWall : TorchWallBase, IWorldImageProvider
    {
        private const string ImageNormal = "Wall_Dungeon";
        private const string ImageBottom = "Wall_Dungeon_Bottom_Torch";
        private const string ImageRight = "Wall_Dungeon_Right_Torch";
        private const string ImageBottomRight = "Wall_Dungeon_Bottom_Right_Torch";
        private const string ImageCorner = "Wall_Dungeon_Corner";

        private readonly AnimationsBatchManager animationsManager;

        public override string Name => "Dungeon Wall";

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is DungeonWall || mapObject is DungeonTorchWall || mapObject is DungeonDoor;
        }

        public DungeonTorchWall()
        {
            animationsManager = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.Random);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (!HasConnectedTile(0, 1) && !HasConnectedTile(1, 0))
            {
                return animationsManager.GetImage(storage, ImageBottomRight);
            }

            if (!HasConnectedTile(0, 1))
            {
                return animationsManager.GetImage(storage, ImageBottom);
            }

            if (!HasConnectedTile(1, 0))
            {
                return animationsManager.GetImage(storage, ImageRight);
            }

            if (!HasConnectedTile(1, 1))
            {
                return storage.GetImage(ImageCorner);
            }

            return storage.GetImage(ImageNormal);
        }
    }
}