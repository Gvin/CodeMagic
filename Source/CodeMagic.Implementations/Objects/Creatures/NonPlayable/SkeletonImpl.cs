using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Objects.Creatures.Implementations;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Creatures.NonPlayable
{
    public class SkeletonImpl : SkeletonCreatureObject, IWorldImageProvider
    {
        private const string ImageUp = "Creature_Skeleton_Up";
        private const string ImageDown = "Creature_Skeleton_Down";
        private const string ImageLeft = "Creature_Skeleton_Left";
        private const string ImageRight = "Creature_Skeleton_Right";

        public SkeletonImpl(SkeletonCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            switch (Direction)
            {
                case Direction.North:
                    return storage.GetImage(ImageUp);
                case Direction.South:
                    return storage.GetImage(ImageDown);
                case Direction.West:
                    return storage.GetImage(ImageLeft);
                case Direction.East:
                    return storage.GetImage(ImageRight);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}