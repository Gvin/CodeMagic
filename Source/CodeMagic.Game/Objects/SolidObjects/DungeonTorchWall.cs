using System;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
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

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is DungeonWall || mapObject is DungeonTorchWall || mapObject is DungeonDoor;
        }

        public DungeonTorchWall(SaveData data) : base(data)
        {
            animationsManager = new AnimationsBatchManager(TimeSpan.FromMilliseconds(300),
                AnimationFrameStrategy.OneByOneStartFromRandom);
        }

        public DungeonTorchWall()
            : base("Dungeon Wall")
        {
            animationsManager = new AnimationsBatchManager(TimeSpan.FromMilliseconds(300),
                AnimationFrameStrategy.OneByOneStartFromRandom);
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