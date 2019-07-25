using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Objects.Creatures.Implementations;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation.Creatures.NonPlayable
{
    public class GoblinImpl : GoblinCreatureObject, IImageProvider
    {
        private const string ImageUp = "Creature_Goblin_Up";
        private const string ImageDown = "Creature_Goblin_Down";
        private const string ImageLeft = "Creature_Goblin_Left";
        private const string ImageRight = "Creature_Goblin_Right";

        public GoblinImpl(GoblinCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            switch (Direction)
            {
                case Direction.Up:
                    return storage.GetImage(ImageUp);
                case Direction.Down:
                    return storage.GetImage(ImageDown);
                case Direction.Left:
                    return storage.GetImage(ImageLeft);
                case Direction.Right:
                    return storage.GetImage(ImageRight);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}